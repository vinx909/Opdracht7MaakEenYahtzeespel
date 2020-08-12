using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht7MaakEenYahtzeespel
{
    public class ButtonFunction
    {
        object button;
        Action<object> function;
        object functionParameter;

        public ButtonFunction(object button, Action<object> function, object functionParameter)
        {
            this.button = button;
            this.function = function;
            this.functionParameter = functionParameter;
        }
        public bool IsSameButton(object button)
        {
            if (this.button == button)
            {
                return true;
            }
            return false;
        }
        public void DoFunction()
        {
            function.Invoke(functionParameter);
        }
        public void SetFunctionParameter(object functionParameter)
        {
            this.functionParameter = functionParameter;
        }
        public object GetFunctionParameter()
        {
            return functionParameter;
        }
    }
}
