using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Numerics;

namespace PixBits.Core {
    public static class Generator {
        
        public static PixBitPackage GeneratePackage(
            double value, 
            uint objectId, 
            uint salt, 
            string agentName, 
            string description, 
            string urlSafeName, // Explicitly pass the checked/validated subname
            bool isElite = false) 
        {
            BigInteger scrambled = Encoder.PackAndScramble(value, objectId, salt);

            // 1. Setup the Manifest
            var manifest = new AgentManifest {
                Name = agentName,
                Description = description,
                IsActive = true,
                TeeAttestation = isElite ? "fips-201-3-active" : "none",
                Services = new List<PixBitService>()
            };

            // 2. Add Standard Service - Now correctly targeting pixbits.base.eth
            manifest.Services.Add(new PixBitService { 
                Name = "mcp", 
                Type = "RPC", 
                Endpoint = $"https://{urlSafeName}.pixbits.base.eth/", // Uniform routing anchor
                Version = "2026-03-03"
            });

            // 3. Add Elite Service if applicable
            if (isElite) {
                manifest.Services.Add(new QuantumService {
                    Name = "Quantum-Isolation-Shield",
                    Type = "QuantumChannel",
                    ServiceClass = "quantum",
                    Endpoint = "pqc-secure-tunnel-v1",
                    EncryptionType = "ML-KEM-768",
                    SignatureType = "ML-DSA-65", // Enforce quantum signature parameters explicitly
                    PIV_Profile = "FIPS-201-3-PRO",
                    HardwareBinding = "TEE-Required",
                    Version = "1.0.0"
                });
            }

            // 4. Return the complete package using the manifest we just built
            return new PixBitPackage {
                Version = "1.1.0",
                Cid = "bafy-pixbits-" + Guid.NewGuid().ToString()[..8],
                VisualManifest = new VisualData {
                    GridSize = "4x4",
                    PixelHex = scrambled.ToString("X") 
                },
                Recovery = new RecoveryConfig {
                    Threshold = 2,
                    ModulusP = Encoder.P.ToString()
                },
                Agent = manifest
            };
        }

        public static string GenerateMetadataJson(double value, uint id, uint salt) {
            var rawInput = Encoder.PackOnly(value, id, salt);
            var meta = new {
                OriginalValue = value,
                Id = id,
                Salt = salt,
                RawInputDecimal = rawInput.ToString(),
                RawInputHex = rawInput.ToString("X")
            };
            return JsonSerializer.Serialize(meta, new JsonSerializerOptions { WriteIndented = true });
        }

        public static string ToJson(PixBitPackage package) {
            return JsonSerializer.Serialize(package, new JsonSerializerOptions { 
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}