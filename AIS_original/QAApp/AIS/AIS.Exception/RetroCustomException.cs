using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ZurichNA.AIS.ExceptionHandling
{

    [Serializable]
    public class RetroDatabaseException : RetroBaseException
    {

        public RetroDatabaseException()
            : base()
        {
            this.setErrorMessage();
        }

        public RetroDatabaseException(string message)
            : base(message)
        {
            this.setErrorMessage();
        }

        public RetroDatabaseException(string message, Exception inner)
            : base(message, inner)
        {
            this.setErrorMessage();
        }

        protected RetroDatabaseException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }

        private void setErrorMessage()
        {
            this.generalError = "Database Error";
            this.suggestedAction = "Database Call Failed. <br /> " +
                                   "Please call the Global IT Service Desk for assistance.";
        }
    }

    /// <summary>
    /// 
    /// </summary>

    [Serializable]
    public class RetroLoginException : RetroBaseException
    {
        public RetroLoginException()
            : base()
        {
            this.setErrorMessage();
        }
        public RetroLoginException(string message)
            : base(message)
        {
            this.setErrorMessage();
        }
        public RetroLoginException(string message, Exception inner) :
            base(message, inner)
        {
            this.setErrorMessage();
        }
        protected RetroLoginException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            this.setErrorMessage();
        }

        private void setErrorMessage()
        {
            this.generalError = "User may not have rights to access Retro Application.";
            this.suggestedAction = "Your User ID may need to be added to Retro Application user group. <br /> " +
                                     "Please contact your BU PC Coordinator for assistance.";
        }

    }

}