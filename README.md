# Draft: Project PixBit

## Hardware-Bound Autonomy

Most AI agents are "Ghost Protocols" that can be copied, deleted, or corrupted by a single bit-flip. PixBit treats an Agent as a physical entity. We use the Ethereum [ERC-8004](https://eips.ethereum.org/EIPS/eip-8004#validation-registry) standard as a "Soul" (On-chain) and [FIPS 201-3 (PIV)](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.201-3.pdf) as the "Body" (Hardware).

### The "Triple-Lock" Architecture (Why it matters)

Layer     | Component                        | The "Why"
Digital   | ERC-8004 Manifest                | Provides a universal, IPFS-backed "Birth Certificate" for the agent.
Visual    | Pixelized Validation Bits Scheme | A 16-hex horizontal bitmask that allows humans to verify an agent's ID at a glance.
Physical  | PIV APDU Script                  | "Soul-binds" the agent to a JavaCard. The private keys never leave the silicon.

### Post-Quantum Resilience (The Elite Tier)We don't just secure agents for today; we secure them for the "Y2Q" (Years to Quantum) era

    - Standard: Uses classical RPC and SECP256K1.
    - Elite: Injects QuantumService endpoints into the manifest, signaling peers to use ML-KEM-768 (Kyber) and ML-DSA (Dilithium) handshakes
    
### Key Features in this Demo

    1. The Generator <(Choice 1)>: Mints agents on Base with an optional "Elite" toggle.
    2. The Scanner <(Choice 2)>: Validates local manifests and checks for a "Hardware Bond" (The APDU link).
    3. Virtual PIV Card <(Choice 6)>: Exports a standalone HTML card that simulates the Secure Element's terminal and visual fingerprint.
    4. Provisioning Script <(Choice 7)>: Generates the raw machine code (APDU) to bridge from software to a physical Smart Card.
    
### Important Technical Details

    - Bit-Flip Protection: The Encoder.cs uses a static modulus $P$ and a pack-and-scramble logic to ensure that a single bit of corruption in the metadata renders the agent invalid, preventing "Zombie Agents."
    - Polymorphic Services: The JSON schema uses JsonDerivedType to allow the manifest to grow. You can add a "Payment Service," a "Quantum Service," or a "Kill-Switch Service" without breaking the base standard.
