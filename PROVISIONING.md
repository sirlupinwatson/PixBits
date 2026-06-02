# PixBit Hardware Provisioning (Internal v1.1)

## Overview

This document describes how to move a PixBit Agent from the C# CLI to a Physical FIPS 201-3 (PIV) Smart Card.

## Hardware Requirements

1. **JavaCard:** NXP JCOP or similar (NIST FIPS 201-3 compliant).
2. **Reader:** Any PC/SC compatible USB reader (e.g., HID OMNIKEY).
3. **Tool:** [GlobalPlatformPro](https://github.com/martinpaljak/GlobalPlatformPro) or `gpshell`.

## The "Soul-Binding" Process

1. **Minting:** Run `Choice 1` in the CLI to create the Agent on Base.
2. **Generating the Receipt:** Run `Choice 7` to generate `Provisioning_ID.apdu`.
3. **Flashing:**
   - Connect reader and insert card.
   - Run: `gp --shell Provisioning_ID.apdu`
  
## Memory Map (OIDs)

- **0x3000 (CHUID):** Stores the IPFS/Filecoin CID.
- **0x9C (Signature):** Holds the Agent's ML-DSA Private Key (Generated on-chip).
