using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ZurichNA.AIS.ExceptionHandling
{

    [Serializable]
    public class RetroBaseException : ApplicationException
    {
        public RetroBaseException()
            : base()
        { }

        public RetroBaseException(string message)
            : base(message)
        {
            //this.generalError = generalError;
            //this.suggestedAction = suggestedAction;
        }

        public RetroBaseException(string message, Exception inner) :
            base(message, inner)
        {

        }

        protected RetroBaseException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }

        public override void GetObjectData(SerializationInfo info,
                       StreamingContext context)
        {
            info.AddValue("generalError", generalError, typeof(String));
            info.AddValue("suggestedAction", suggestedAction, typeof(String));
            base.GetObjectData(info, context);
        }

        private void setErrorMessage()
        {

        }

        protected string generalError = "General Error";
        protected string suggestedAction = "General Error. <br /> " +
                      "Please call the Global IT Service Desk for assistance.";

        public string GeneralError
        {
            get { return generalError; }
            set { generalError = value; }
        }
        public string SuggestedAction
        {
            get { return suggestedAction; }
            set { suggestedAction = value; }
        }
    }

}