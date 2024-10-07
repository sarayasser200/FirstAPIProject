namespace FirstAPIProject
{
    public class JwtOptions { 
        public string Issure { get; set; }
        public string Audience { get; set; }
        public int Liftetime { get; set; }
        public string SigningKey { get; set; }
    }

}
