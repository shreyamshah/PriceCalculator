using System;
using System.Collections.Generic;
using System.Text;

namespace PriceCalculator.Helper
{
    public interface IAuthHelper
    {
        void Authenticate(int requestCode = 0);
    }
}
