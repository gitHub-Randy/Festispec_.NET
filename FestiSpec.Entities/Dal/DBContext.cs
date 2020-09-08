using FestiSpec.Entities.Dal;
using FestiSpec.Entities.Offline;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FestiSpec.Entities.Dal
{
    public sealed class DBContext
    {
        public static bool IsOnline => instance.IsOnline;
        public static bool DoSync { get; set; } = true;
        private static IFestiSpecData instance = null;
        private static readonly object padlock = new object();

        public static IFestiSpecData Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        try
                        {
                            //Attempt to connect to DB
                            instance = new FestiSpecContext();
                            var accounts = instance.EmployeeAccounts.ToList();
                            Console.WriteLine(accounts);
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e.StackTrace);
                            if (DoSync)
                            {
                                //Connection went wrong
                                //Attempt to load serialized data
                                MessageBox.Show("De applicatie is in offline modus gestart.\nVeranderingen worden niet opgeslagen!", "Offline Modus");
                                instance = new SerializedFestiSpecContext();
                            }
                        }
                    }
                    return instance;
                }
            }
        }
    }
}
