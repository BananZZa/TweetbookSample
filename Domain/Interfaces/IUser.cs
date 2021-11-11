namespace Domain.Interfaces
{
    public interface IUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
    }
}