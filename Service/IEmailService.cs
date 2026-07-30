using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackerProject.Service
{
    public interface IEmailService
    {
        Task<bool> EmailSend(string email);
    }
}