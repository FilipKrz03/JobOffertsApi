namespace UsersService.Dto
{
    public record TokenResponseFromRefreshFirebaseActionDto(string Id_token, string Expires_in, string Refresh_token) { }
}
