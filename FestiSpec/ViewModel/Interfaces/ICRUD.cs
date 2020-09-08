using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FestiSpec.ViewModel.Interfaces
{
    public interface ICRUD
    {
        void Add();
        void Update();
        void Remove();
        void View();
    }
}