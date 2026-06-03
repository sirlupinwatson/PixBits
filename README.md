# PixBit: Hardware-Bound Agent Autonomy

Most AI agents are "Ghost Protocols" that can be copied, deleted, or corrupted by a single bit-flip. PixBit treats an autonomous agent as an immutable physical entity. We combine the Ethereum ERC-8004 validation registry standard with FIPS 201-3 (PIV) protocols to bind an agent's operational "Soul" (On-chain) to a hardened "Body" (Hardware Secure Element).

---

## The "Triple-Lock" Architecture

| Layer | Component | The "Why" |
| :--- | :--- | :--- |
| **Digital** | ERC-8004 Manifest | Provides a universal, content-addressed "Birth Certificate" for the agent. |
| **Visual** | PixBits Bitmask Scheme | A 16-hex horizontal visual bitmask allowing humans to verify agent authenticity at a glance. |
| **Physical** | PIV APDU Scripting | "Soul-binds" the agent to a physical JavaCard. The root signing keys never leave the silicon. |

---

## Post-Quantum Resilience (The Elite Tier)

We secure identities against post-quantum intercept-now-decrypt-later vectors by establishing native fallback handshakes right inside the initialization packet:

* **Standard Tier:** Utilizes classical RPC routing bridges and SECP256K1 signing curves.
* **Elite Tier:** Injects specialized `QuantumService` parameters directly into the manifest, forcing peer nodes to execute handshakes via **ML-KEM-768** (Kyber) and **ML-DSA-65** (Dilithium) standards.

---

## Core Features (v1.1.0 Engine Demo)

1. **Agent Generation (`Choice 1`):** Computes visual validation bitmasks and registers cryptographic proofs to our `PixBitsVerifier` contract on Base.
2. **Local Manifest Scanner (`Choice 2`):** Validates offline manifest states and checks for active secure-element hardware bindings.
3. **Virtual PIV Token Card (`Choice 6`):** Exports an interactive HTML runtime environment simulating the card's terminal communication matrix.
4. **Hardware Provisioning (`Choice 7`):** Generates raw machine-level Application Protocol Data Units (APDU) scripts to program physical smart cards.

---

## Technical Guardrails

* **Anti-Zombie Protection:** `Encoder.cs` uses a static prime modulus $P$ and a pack-and-scramble mechanism. A single bit-flip or unauthorized mutation of metadata breaks the algebraic proof, instantly stripping the agent of its "Authentic" network routing state.
* **Polymorphic Extensions:** Built on top of `.NET` polymorphic JSON serialization, allowing the manifest footprint to branch out. New service layers (e.g., automated execution wallets or kill-switch triggers) can be added without breaking legacy parsing engines.

---

## Upcoming Development Roadmap (v1.2.0)

The next developmental branch moves beyond identity creation and focuses heavily on secure network-level routing and namespace management:

* [ ] **On-Chain Corporate Brand Reservations:** Implementing a smart-contract level string protection registry to prevent endpoint squatting on high-value names (e.g., `google.pixbits.base.eth`).
* [ ] **Native Basename Ownership Validation:** Transitioning the CLI namespace checks from mock web APIs to direct Nethereum contract calls, verifying that the minting wallet explicitly holds the underlying Base Name domain token before allowing an identity stamp.
* [ ] **Built-in Multi-Sig & Wallet Synchronization:** Embedding a low-level key management system directly inside the runner to combine domain registration and identity proofing into a single atomic transaction flow.
