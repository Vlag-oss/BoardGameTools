using BoardGameTools.Application.Common.Exceptions;

namespace BoardGameTools.Application.Auth.ResetPassword
{
    public class SamePasswordException : BusinessException
    {
        public SamePasswordException() : base("Le nouveau mot de passe ne peut pas être le même que l'ancien.")
        {
            
        }
    }
}
