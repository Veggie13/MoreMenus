using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;

namespace MoreMenus
{
    public interface IMenuAction
    {
        void Fire(DTE2 applicationObj);
    }
}
