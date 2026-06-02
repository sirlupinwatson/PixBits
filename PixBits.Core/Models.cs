using System.Text.Json.Serialization;

namespace PixBits.Core {
        public class PixBitPackage {
        public string Version { get; set; } = "1.1.0";
        public string Cid { get; set; } = string.Empty; 
        
        // These link to the other classes in this file
        public required VisualData VisualManifest { get; set; }
        public required RecoveryConfig Recovery { get; set; } 
        public required AgentManifest Agent { get; set; } 
    }

    // 1. THE BASE CLASS (The "Interface" for all services)
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "ServiceClass")]
    [JsonDerivedType(typeof(PixBitService), "standard")]
    [JsonDerivedType(typeof(QuantumService), "quantum")]
    public class PixBitService {
        public string Type { get; set; } = "BaseRegistry"; // Defaulting to the Base identity architecture
        public string Name { get; set; } = "PixBits Base Agent";
        public string Version { get; set; } = "1.1.0";
        public string ServiceClass { get; set; } = "";
        
        // Fallback RPC or Gateway for resolving the metadata off-chain if needed
        public string Endpoint { get; set; } = "https://api.basenames.xyz";
    }

    // 2. THE SPECIALIZED CLASS
    public class QuantumService : PixBitService {
        public string EncryptionType { get; set; } = "ML-KEM-768";
        public string SignatureType { get; set; } = "ML-DSA-65";
        public string PIV_Profile { get; set; } = "FIPS-201-3-PRO";
        public string HardwareBinding { get; set; } = "TEE-Required";

        public QuantumService() {
            Type = "QuantumChannel";
            ServiceClass = "quantum"; // Automatically overrides the base empty string
        }
    }

    // 3. THE AGENT MANIFEST (Using the Base Class for the List)
    public class AgentManifest {
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    
    // Dynamic instance generation tracking
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

    public List<PixBitService> Services { get; set; } = new(); 
    
    public bool IsActive { get; set; } = true;
    public string TeeAttestation { get; set; } = "none";
}

    public class VisualData {
        public string GridSize { get; set; } = "4x4";
        public string? PixelHex { get; set; } 
    }

    public class RecoveryConfig {
        public int Threshold { get; set; }
        public string? ModulusP { get; set; } 
    }
}