using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loggly.Example
{

    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;
        private Form _form;

        public WaitCursor(Form form)
        {
            _form = form;
            _previousCursor = form.Cursor;

            form.Cursor = Cursors.WaitCursor;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _form.Cursor = _previousCursor;
        }

        #endregion
    }
}
