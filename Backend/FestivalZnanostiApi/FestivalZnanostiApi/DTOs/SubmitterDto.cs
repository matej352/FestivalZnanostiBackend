namespace FestivalZnanostiApi.DTOs
{
    public class SubmitterDto
    {
        public string Email { get; set; }

        public string? Password { get; set; }    // password je opcionalan, ako postoji stvara se submitter u bazi i nakon logina, user će moći editirati vlastite evente, inače neće
    }
}
