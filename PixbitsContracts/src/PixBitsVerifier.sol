// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

contract PixBitsVerifier {
    uint256 public constant P = 340282366920938463463374607431768211455;
    uint256 public constant K = 314159265358979323846264338327950288419;

    mapping(uint256 => uint256) public pixelToCommitment; // Maps Visual to Hidden Hash

    event PixBitMinted(uint256 indexed pixelData, uint256 commitment);

    function verify(uint256 pixelData, uint256 rawInput) public pure returns (bool) {
        return (rawInput * K) % P == pixelData;
    }

    // New Registration: Accepts the pixelData (Public) and a commitment (Hash of Secret)
    function registerWithPrivacy(uint256 pixelData, uint256 rawInput, uint256 secretCommitment) public {
        require(verify(pixelData, rawInput), "Invalid Visual Proof");
        require(pixelToCommitment[pixelData] == 0, "Already Minted");

        pixelToCommitment[pixelData] = secretCommitment;
        emit PixBitMinted(pixelData, secretCommitment);
    }
}
