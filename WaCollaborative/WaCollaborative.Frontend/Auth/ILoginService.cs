﻿namespace WaCollaborative.Frontend.Auth
{
    public interface ILoginService
    {
        public Task LoginAsync(string token);

        public Task LogoutAsync();
    }
}