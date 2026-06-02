using System.Numerics;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace PixBits.Core {
    public class PixBitsProtocol {
        // Update contract id for each update
        private const string ContractAddress = "0xA33Ee445466377Be3775405F261a9d1651dE7fAf";

        // Updated ABI to match 'registerWithPrivacy'
        private const string Abi = @"[
            {
                'type':'function',
                'name':'pixelToCommitment',
                'inputs':[{'name':'','type':'uint256'}],
                'outputs':[{'name':'','type':'uint256'}],
                'stateMutability':'view'
            },
            {
                'type':'function',
                'name':'verify',
                'inputs':[{'name':'pixelData','type':'uint256'},{'name':'originalPacked','type':'uint256'}],
                'outputs':[{'name':'','type':'bool'}],
                'stateMutability':'pure'
            },
            {
                'type':'function',
                'name':'registerWithPrivacy',
                'inputs':[{'name':'pixelData','type':'uint256'},{'name':'rawInput','type':'uint256'},{'name':'secretCommitment','type':'uint256'}],
                'outputs':[],
                'stateMutability':'nonpayable'
            }
        ]";

        private readonly Web3 _web3;

        public PixBitsProtocol(string rpcUrl, string privateKey) {
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            _web3 = new Web3(account, rpcUrl);
        }

        public async Task<string> RegisterOnChain(string pixelHex, BigInteger rawInput, BigInteger commitment) {
            var pixelData = BigInteger.Parse("0" + pixelHex, System.Globalization.NumberStyles.HexNumber);
            var contract = _web3.Eth.GetContract(Abi, ContractAddress);
            var registerFunc = contract.GetFunction("registerWithPrivacy");

            Console.WriteLine("Sending Secure Transaction to Base...");

            var txHash = await registerFunc.SendTransactionAsync(
                _web3.TransactionManager.Account.Address,
                new Nethereum.Hex.HexTypes.HexBigInteger(250000), 
                new Nethereum.Hex.HexTypes.HexBigInteger(0),
                pixelData, 
                rawInput,
                commitment
            );

            // Wait for receipt to ensure it's mined
            // Use 2000ms because Base produces a block every 2 seconds. 
            // Checking more often than that just wastes bandwidth.
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
            int attempts = 0;

            while (receipt == null && attempts < 30) { 
                await Task.Delay(2000); 
                receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
                attempts++;
                Console.Write("."); // Visual feedback that we are waiting
            }

            if (receipt == null) {
                throw new Exception("Transaction timeout: Not found on Base after 60 seconds.");
            }

            // Status 1 = Success, Status 0 = Revert/Failure
            if (receipt.Status.Value == 0) {
                throw new Exception("Transaction REVERTED on-chain. Check gas or contract logic.");
            }

            Console.WriteLine(" [Confirmed]"); 
            return txHash;
        }

        public async Task<bool> VerifyOnChain(string pixelHex, BigInteger originalPacked) {
            var pixelData = BigInteger.Parse("0" + pixelHex, System.Globalization.NumberStyles.HexNumber);
            var contract = _web3.Eth.GetContract(Abi, ContractAddress);
            var verifyFunction = contract.GetFunction("verify");
            return await verifyFunction.CallAsync<bool>(pixelData, originalPacked);
        }

        public async Task<bool> VerifyIdentityOwnership(string pixelHex, string secretData) {
            // 1. Re-generate the commitment from the secret locally
            BigInteger localCommitment = Encoder.GenerateCommitment(secretData);

            // 2. Format the pixelData to match on-chain uint256
            var pixelData = BigInteger.Parse("0" + pixelHex, System.Globalization.NumberStyles.HexNumber);

            // 3. Query the contract mapping: pixelToCommitment[pixelData]
            var contract = _web3.Eth.GetContract(Abi, ContractAddress);
            var mappingFunc = contract.GetFunction("pixelToCommitment");
            
            // This is a "Call", not a transaction (Free)
            BigInteger onChainCommitment = await mappingFunc.CallAsync<BigInteger>(pixelData);

            // 4. Validate
            if (onChainCommitment == 0) {
                Console.WriteLine("❌ This CID has not been registered on-chain.");
                return false;
            }

            return localCommitment == onChainCommitment;
        }

        public async Task<string> GetCommitmentByHex(string pixelHex) {
            // Convert Hex string to the BigInt format the contract needs
            var pixelData = BigInteger.Parse("0" + pixelHex, System.Globalization.NumberStyles.HexNumber);

            var contract = _web3.Eth.GetContract(Abi, ContractAddress);
            var mappingFunc = contract.GetFunction("pixelToCommitment");

            // Call the contract
            BigInteger result = await mappingFunc.CallAsync<BigInteger>(pixelData);

            return result.ToString(); // Returns the decimal Commitment

        }
    }
}