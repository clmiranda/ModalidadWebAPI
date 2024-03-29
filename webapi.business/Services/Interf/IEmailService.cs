﻿using System.Threading.Tasks;

namespace webapi.business.Services.Interf
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string html);
    }
}
