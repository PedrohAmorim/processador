using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Processador
{
    public class SqlDataBase
    {

        public static SqlConnection getConnection(string database, bool open)
        {

            var conn = new SqlConnection(string.Format(ConfigurationManager.AppSettings.Get("SQLCONNECTION"), database));

            if (open)
            {
                conn.Open();
            }


            return conn;

        }


        public static bool InsertEventFromOrganization(Classes.Event Event)
        {
            try
            {
                using (SqlConnection conn = getConnection(Event._module.Database, true)) {
                    string query =
                  @"INSERT INTO [dbo].[Events] ([UnitId], [EventId], [StartDateTime], [StartOdometer], [EndDateTime], [EndOdometer], [Value], [TotalTimeSeconds], [assetid], [DriverId], [driveridpk])
              VALUES(@unitid,@eventid,@startdatetime,@startodometer,@enddatetime,@endodometer,@value,@totalseconds,@assetid,'0',(select top 1 id from drivers where employee_number  = 0 ))";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add(new SqlParameter("@unitid", Event._header.UnitId));
                    cmd.Parameters.Add(new SqlParameter("@eventid", Event.EventId));
                    cmd.Parameters.Add(new SqlParameter("@startdatetime", Event.timestampStart));
                    cmd.Parameters.Add(new SqlParameter("@startodometer", Event.odometerStart));
                    cmd.Parameters.Add(new SqlParameter("@enddatetime", Event.timestampEnd));
                    cmd.Parameters.Add(new SqlParameter("@endodometer", Event.odometerEnd));
                    cmd.Parameters.Add(new SqlParameter("@value", Event.maxRpm));
                    cmd.Parameters.Add(new SqlParameter("@totalseconds", (Event.timestampEnd - Event.timestampStart).TotalSeconds));
                    cmd.Parameters.Add(new SqlParameter("@assetid", Event._module.AssetId));

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool InsertTrackFromOrganization(Classes.Track track)
        {
            try
            {
                using (SqlConnection conn = getConnection(track._module.Database, true))
                {
                    string query =
                  @"INSERT INTO [dbo].[Positions] ( [UnitId], [Timestamp], [Latitude], [Longitude], [SpeedKilometresPerHour], [Orientation], [assetid], [tipodispositivo], [istrack], [DriverId], [driveridpk],[IgnitionOn])
                    VALUES(@unitid,@timestamp,@latitude,@longitude,@speed,@orientation,@assetid,@tipo,@istrack,'0',(select top 1 id from drivers where employee_number  = 0 ),0)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add(new SqlParameter("@unitid", track._header.UnitId));
                    cmd.Parameters.Add(new SqlParameter("@timestamp", track.timestamp));
                    cmd.Parameters.Add(new SqlParameter("@latitude", track.latitude));
                    cmd.Parameters.Add(new SqlParameter("@longitude", track.longitude));
                    cmd.Parameters.Add(new SqlParameter("@speed", track.speed));
                    cmd.Parameters.Add(new SqlParameter("@orientation", track.orientation));            
                    cmd.Parameters.Add(new SqlParameter("@assetid", track._module.AssetId));
                    cmd.Parameters.Add(new SqlParameter("@tipo", 5));
                    cmd.Parameters.Add(new SqlParameter("@istrack", true));

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static SqlConnection connectPlugAndPlay(bool open)
        {
            var conn = new SqlConnection(ConfigurationManager.AppSettings.Get("PLUGANDPLAY"));

            if (open) { conn.Open(); }

            return conn;
        }


    }
}
