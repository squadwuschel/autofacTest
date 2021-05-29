using Squad.AutofacTest.Models.Interfaces;

namespace Squad.AutofacTest.Models
{
    public class TodoModelBuilder : ITodoModelBuilder
    {

        public string Erweiternt(string text)
        {
            return $"{text} + {text}";
        }
        
    }
}
