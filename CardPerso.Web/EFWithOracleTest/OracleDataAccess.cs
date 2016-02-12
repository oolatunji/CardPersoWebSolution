using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EFWithOracleTest
{
    public class OracleDataAccess
    {

        public OracleConnection connect(string host, string port, string service, string userId, string password)
        {
            try
            {
                var conn = new OracleConnection();
                conn.ConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0} )(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}", host, port, service, userId, password);
                conn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nConnected to Oracle.");
                return conn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void createTables(OracleConnection conn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Creating database tables");

            string query = @"CREATE TABLE SYSTEMUSERS
                               (	ID NUMBER(*,0) NOT NULL ENABLE, 
	                                OTHERNAMES VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	                                LASTNAME VARCHAR2(50 BYTE) NOT NULL ENABLE, 
	                                GENDER VARCHAR2(20 BYTE) NOT NULL ENABLE, 
	                                EMAILADDRESS VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	                                USERNAME VARCHAR2(50 BYTE) NOT NULL ENABLE, 
	                                PASSWORD VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	                                ROLEID NUMBER(*,0) NOT NULL ENABLE, 
	                                CREATEDON DATE NOT NULL ENABLE, 
	                                CONSTRAINT SYSTEMUSER_PK PRIMARY KEY (ID)
                               )";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMUSERS successfully created");

            query = @"CREATE TABLE SYSTEMROLES
                        (	
                          ID NUMBER(*,0) NOT NULL ENABLE, 
                          NAME VARCHAR2(100 BYTE) NOT NULL ENABLE, 
                          CONSTRAINT SYSTEMROLES_PK PRIMARY KEY (ID)
                        )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMROLES successfully created");

            query = @"CREATE TABLE SYSTEMROLEFUNCTIONS 
                       (	
                            ROLEID NUMBER(*,0) NOT NULL ENABLE, 
                            FUNCTIONID NUMBER(*,0) NOT NULL ENABLE
                       )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMROLEFUNCTIONS successfully created");

            query = @"CREATE TABLE SYSTEMIDMANAGER 
                       (	
                            USERID NUMBER(*,0), 
                            FUNCTIONID NUMBER(*,0), 
                            ROLEID NUMBER(*,0), 
                            ROLEFUNCTIONID NUMBER(*,0), 
                            APPROVALID NUMBER(*,0), 
                            ID NUMBER
                       )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMIDMANAGER successfully created");

            query = @"CREATE TABLE SYSTEMFUNCTIONS 
                       (	
                           ID NUMBER(*,0) NOT NULL ENABLE, 
	                       NAME VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	                       PAGELINK VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	                       CONSTRAINT SYSTEMFUNCTIONS_PK PRIMARY KEY (ID)
                       )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMFUNCTIONS successfully created");

            query = @"CREATE TABLE SYSTEMAUDITTRAIL 
                       (	
                          ID NUMBER(*,0) NOT NULL ENABLE, 
                          TYPE VARCHAR2(100 BYTE) NOT NULL ENABLE, 
                          DETAILS VARCHAR2(1000 BYTE) NOT NULL ENABLE, 
                          REQUESTEDBY VARCHAR2(20 BYTE) NOT NULL ENABLE, 
                          APPROVEDBY VARCHAR2(20 BYTE) NOT NULL ENABLE, 
                          REQUESTEDON DATE NOT NULL ENABLE, 
                          APPROVEDON DATE NOT NULL ENABLE, 
                          CONSTRAINT SYSTEMAUDITTRAIL_PK PRIMARY KEY (ID)
                        )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMAUDITTRAIL successfully created");

            query = @"CREATE TABLE SYSTEMAPPROVALS 
                       (	
                          ID NUMBER(*,0) NOT NULL ENABLE, 
                          TYPE VARCHAR2(100 BYTE) NOT NULL ENABLE, 
                          DETAILS VARCHAR2(1000 BYTE) NOT NULL ENABLE, 
                          OBJ VARCHAR2(1000 BYTE) NOT NULL ENABLE, 
                          REQUESTEDBY VARCHAR2(20 BYTE) NOT NULL ENABLE, 
                          REQUESTEDON DATE NOT NULL ENABLE, 
                          APPROVEDBY VARCHAR2(20 BYTE), 
                          APPROVEDON DATE, 
                          STATUS VARCHAR2(50 BYTE) NOT NULL ENABLE, 
                          CONSTRAINT SYSTEMAPPROVALS_PK PRIMARY KEY (ID)
                       )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMAPPROVALS successfully created");

            query = @"CREATE TABLE SYSTEMAPPROVALCONFIGURATION 
                       (	
                          ID NUMBER(*,0) NOT NULL ENABLE, 
                          TYPE VARCHAR2(50 BYTE) NOT NULL ENABLE, 
                          APPROVE NUMBER(*,0) NOT NULL ENABLE
                       )";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Table SYSTEMAPPROVALCONFIGURATION successfully created");
        }

        public void createSequences(OracleConnection conn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCreating database sequences");

            string query = @"CREATE SEQUENCE SYSTEMUSER_SEQ  
                                MINVALUE 1 MAXVALUE 9999999999999999999999999999 
                                INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Sequence SYSTEMUSER_SEQ successfully created");

            query = @"CREATE SEQUENCE SYSTEMROLES_SEQ 
                        MINVALUE 1 MAXVALUE 9999999999999999999999999999 
                        INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Sequence SYSTEMROLES_SEQ successfully created");

            query = @"CREATE SEQUENCE SYSTEMFUNCTIONS_SEQ 
                        MINVALUE 1 MAXVALUE 9999999999999999999999999999 
                        INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Sequence SYSTEMFUNCTIONS_SEQ successfully created");

            query = @"CREATE SEQUENCE SYSTEMAUDITTRAIL_SEQ 
                        MINVALUE 1 MAXVALUE 9999999999999999999999999999 
                        INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Sequence SYSTEMAUDITTRAIL_SEQ successfully created");

            query = @"CREATE SEQUENCE SYSTEMAPPROVALS_SEQ 
                        MINVALUE 1 MAXVALUE 9999999999999999999999999999 
                        INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Sequence SYSTEMAPPROVALS_SEQ successfully created");
        }

        public void createTriggers(OracleConnection conn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCreating database triggers");

            string query = @"create or replace TRIGGER SYSTEMUSER_TRG 
                                BEFORE INSERT ON SYSTEMUSERS 
                                FOR EACH ROW 
                                BEGIN
                                  <<COLUMN_SEQUENCES>>
                                  BEGIN
                                    IF INSERTING AND :NEW.ID IS NULL THEN
                                      SELECT SYSTEMUSER_SEQ.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
                                    END IF;
                                  END COLUMN_SEQUENCES;
                                END;";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Trigger SYSTEMUSER_TRG successfully created");

            query = @"create or replace TRIGGER SYSTEMROLES_TRG 
                        BEFORE INSERT ON SYSTEMROLES 
                        FOR EACH ROW 
                        BEGIN
                          <<COLUMN_SEQUENCES>>
                          BEGIN
                            IF INSERTING AND :NEW.ID IS NULL THEN
                              SELECT SYSTEMROLES_SEQ.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
                            END IF;
                          END COLUMN_SEQUENCES;
                        END;";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Trigger SYSTEMROLES_TRG successfully created");

            query = @"create or replace TRIGGER SYSTEMFUNCTIONS_TRG 
                        BEFORE INSERT ON SYSTEMFUNCTIONS 
                        FOR EACH ROW 
                        BEGIN
                          <<COLUMN_SEQUENCES>>
                          BEGIN
                            IF INSERTING AND :NEW.ID IS NULL THEN
                              SELECT SYSTEMFUNCTIONS_SEQ.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
                            END IF;
                          END COLUMN_SEQUENCES;
                        END;";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Trigger SYSTEMFUNCTIONS_TRG successfully created");

            query = @"create or replace TRIGGER SYSTEMAUDITTRAIL_TRG 
                        BEFORE INSERT ON SYSTEMAUDITTRAIL 
                        FOR EACH ROW 
                        BEGIN
                          <<COLUMN_SEQUENCES>>
                          BEGIN
                            IF INSERTING AND :NEW.ID IS NULL THEN
                              SELECT SYSTEMAUDITTRAIL_SEQ.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
                            END IF;
                          END COLUMN_SEQUENCES;
                        END;";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Trigger SYSTEMAUDITTRAIL_TRG successfully created");

            query = @"create or replace TRIGGER SYSTEMAPPROVALS_TRG 
                        BEFORE INSERT ON SYSTEMAPPROVALS 
                        FOR EACH ROW 
                        BEGIN
                          <<COLUMN_SEQUENCES>>
                          BEGIN
                            IF INSERTING AND :NEW.ID IS NULL THEN
                              SELECT SYSTEMAPPROVALS_SEQ.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
                            END IF;
                          END COLUMN_SEQUENCES;
                        END;";
            cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|-- Trigger SYSTEMAPPROVALS_TRG successfully created");
        }

        public void setupdatabase(OracleConnection conn)
        {
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                createTables(conn);
                createSequences(conn);
                createTriggers(conn);
                setupSystemData(conn);

                Console.WriteLine("\nSystem Database Successfully Configured");

                txn.Commit();
                close(conn);
            }
            catch(Exception e)
            {
                txn.Rollback();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }

        public void setupSystemData(OracleConnection conn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSetting up system default data.");

            var functions = Function.DefaultFunctions();
            string query = "INSERT INTO SYSTEMFUNCTIONS (NAME, PAGELINK) values(:name, :pagelink)";
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.BindByName = true;

                command.ArrayBindCount = functions.Count;
                command.Parameters.Add(":name", OracleDbType.Varchar2, functions.Select(rf => rf.Name).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":pagelink", OracleDbType.Varchar2, functions.Select(rf => rf.PageLink).ToArray(), ParameterDirection.Input);

                int result = command.ExecuteNonQuery();
                if (result == functions.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\t|-- System Functions data successfully inserted");

                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO SYSTEMROLES(NAME) VALUES(:name) RETURNING ID INTO :id";
                    cmd.Parameters.Add(":name", OracleDbType.Varchar2, "Default Systems Administration", ParameterDirection.Input);

                    OracleParameter outputParameter = new OracleParameter("id", OracleDbType.Int32);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);

                    int rowsInserted = cmd.ExecuteNonQuery();
                    if (rowsInserted > 0)
                    {
                        var roleId = Convert.ToInt32(outputParameter.Value.ToString());
                        var rolefunctions = new List<RoleFunctions>();

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT * FROM SYSTEMFUNCTIONS";
                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Int32 RoleId = roleId;
                            Int32 FunctionId = Convert.ToInt32(dr["ID"]);
                            rolefunctions.Add(new RoleFunctions()
                            {
                                RoleId = RoleId,
                                FunctionId = FunctionId
                            });
                        }

                        SaveRoleFunctions(rolefunctions, conn);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t|-- Default System Administration role successfully inserted");
                    }

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO SYSTEMROLES(NAME) VALUES(:name) RETURNING ID INTO :id";
                    cmd.Parameters.Add(":name", OracleDbType.Varchar2, "Reset Card Print Status", ParameterDirection.Input);

                    outputParameter = new OracleParameter("id", OracleDbType.Int32);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);
                    rowsInserted = cmd.ExecuteNonQuery();
                    if (rowsInserted > 0)
                    {
                        var roleId = Convert.ToInt32(outputParameter.Value.ToString());
                        var rolefunctions = new List<RoleFunctions>();

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT * FROM SYSTEMFUNCTIONS WHERE NAME = 'Reset Card Print Status'";
                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Int32 RoleId = roleId;
                            Int32 FunctionId = Convert.ToInt32(dr["ID"]);
                            rolefunctions.Add(new RoleFunctions()
                            {
                                RoleId = RoleId,
                                FunctionId = FunctionId
                            });
                        }

                        SaveRoleFunctions(rolefunctions, conn);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t|-- Reset Card Print Status role successfully inserted");
                    }

                    SaveApprovalConfigurtions(conn);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\t|-- System Approval Configurations successfully inserted");


                    cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM SYSTEMROLES WHERE NAME = 'Reset Card Print Status'";
                    OracleDataReader roles = cmd.ExecuteReader();
                    Int32 rId = 0;
                    while (roles.Read())
                    {
                        rId = Convert.ToInt32(roles["ID"]);
                        break;
                    }

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT VUSERNAME, VPASSWORD, VOFFICIALEMAIL FROM USERDETAILS WHERE VUSERNAME NOT IN (SELECT USERNAME FROM SYSTEMUSERS)";
                    OracleDataReader usrs = cmd.ExecuteReader();
                    var users = new List<User>();
                    while (usrs.Read())
                    {
                        string username = Convert.ToString(usrs["VUSERNAME"]);
                        string password = Convert.ToString(usrs["VPASSWORD"]);
                        string officialemail = Convert.ToString(usrs["VOFFICIALEMAIL"]);
                        users.Add(new User()
                        {
                            LastName = username,
                            Othernames = username,
                            Gender = "Male",
                            Email = officialemail,
                            Username = username,
                            Password = password,
                            RoleId = rId,
                            CreatedOn = System.DateTime.Now
                        });
                    }

                    SaveUsers(users, conn);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\t|-- System Existing Users successfully migrated");

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM SYSTEMROLES WHERE NAME = 'Default Systems Administration'";
                    roles = cmd.ExecuteReader();
                    rId = 0;
                    while (roles.Read())
                    {
                        rId = Convert.ToInt32(roles["ID"]);
                        break;
                    }

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO SYSTEMUSERS(LASTNAME, OTHERNAMES, GENDER, EMAILADDRESS, USERNAME, PASSWORD, ROLEID, CREATEDON) VALUES(:lastname, :othernames, :gender, :email, :username, :password, :roleid, :createdon) RETURNING ID INTO :id";
                    cmd.Parameters.Add(":lastname", OracleDbType.Varchar2, "Default", ParameterDirection.Input);
                    cmd.Parameters.Add(":othernames", OracleDbType.Varchar2, "Administrator", ParameterDirection.Input);
                    cmd.Parameters.Add(":gender", OracleDbType.Varchar2, "Male", ParameterDirection.Input);
                    cmd.Parameters.Add(":email", OracleDbType.Varchar2, "defaultadmin@gmail.com", ParameterDirection.Input);
                    cmd.Parameters.Add(":username", OracleDbType.Varchar2, "Admin", ParameterDirection.Input);
                    cmd.Parameters.Add(":password", OracleDbType.Varchar2, "password", ParameterDirection.Input);
                    cmd.Parameters.Add(":roleid", OracleDbType.Int32, rId, ParameterDirection.Input);
                    cmd.Parameters.Add(":createdon", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);

                    outputParameter = new OracleParameter("id", OracleDbType.Int32);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);

                    rowsInserted = cmd.ExecuteNonQuery();
                    if (rowsInserted > 0)
                    {
                        var userId = Convert.ToInt32(outputParameter.Value.ToString());

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO USERDETAILS(ID1, VUSERNAME, VPASSWORD, VOFFICIALEMAIL) VALUES(:id, :username, :password, :email)";
                        cmd.Parameters.Add(":id", OracleDbType.Varchar2, userId, ParameterDirection.Input);
                        cmd.Parameters.Add(":username", OracleDbType.Varchar2, "Admin", ParameterDirection.Input);
                        cmd.Parameters.Add(":password", OracleDbType.Varchar2, "password", ParameterDirection.Input);
                        cmd.Parameters.Add(":email", OracleDbType.Varchar2, "defaultadmin@gmail.com", ParameterDirection.Input);

                        rowsInserted = cmd.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\n|-- Default System Admin User successfully Created");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\t|-- Details are as follows:");
                            Console.WriteLine(string.Format("\tUsername: \tAdmin"));
                            Console.WriteLine(string.Format("\tPassword: \tpassword\n"));
                        }
                    }
                }
            }
        }

        public static bool SaveRoleFunctions(List<RoleFunctions> rolefunctions, OracleConnection conn)
        {
            bool saved = false;
            string query = "INSERT INTO SYSTEMROLEFUNCTIONS (ROLEID, FUNCTIONID) values(:roleid, :functionid)";
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.BindByName = true;
                // In order to use ArrayBinding, the ArrayBindCount property
                // of OracleCommand object must be set to the number of records to be inserted
                command.ArrayBindCount = rolefunctions.Count;
                command.Parameters.Add(":roleid", OracleDbType.Int32, rolefunctions.Select(rf => rf.RoleId).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":functionid", OracleDbType.Int32, rolefunctions.Select(rf => rf.FunctionId).ToArray(), ParameterDirection.Input);

                int result = command.ExecuteNonQuery();
                if (result == rolefunctions.Count)
                    saved = true;

                return saved;
            }
        }

        public static bool SaveApprovalConfigurtions(OracleConnection conn)
        {
            var confs = ApprovalConfiguration.DefaultApprovalConfigurations();
            bool saved = false;
            string query = "INSERT INTO SYSTEMAPPROVALCONFIGURATION (ID, TYPE, APPROVE) values(:id, :type, :approve)";
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.BindByName = true;
                // In order to use ArrayBinding, the ArrayBindCount property
                // of OracleCommand object must be set to the number of records to be inserted
                command.ArrayBindCount = confs.Count;
                command.Parameters.Add(":id", OracleDbType.Int32, confs.Select(rf => rf.Id).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":type", OracleDbType.Varchar2, confs.Select(rf => rf.Type).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":approve", OracleDbType.Int32, confs.Select(rf => rf.Approve).ToArray(), ParameterDirection.Input);

                int result = command.ExecuteNonQuery();
                if (result == confs.Count)
                    saved = true;

                return saved;
            }
        }

        public static bool SaveUsers(List<User> users, OracleConnection conn)
        {
            bool saved = false;
            string query = "INSERT INTO SYSTEMUSERS(LASTNAME, OTHERNAMES, GENDER, EMAILADDRESS, USERNAME, PASSWORD, ROLEID, CREATEDON) VALUES(:lastname, :othernames, :gender, :email, :username, :password, :roleid, :createdon)";
            using (var command = conn.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.BindByName = true;
                // In order to use ArrayBinding, the ArrayBindCount property
                // of OracleCommand object must be set to the number of records to be inserted
                command.ArrayBindCount = users.Count;
                command.Parameters.Add(":lastname", OracleDbType.Varchar2, users.Select(rf => rf.LastName).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":othernames", OracleDbType.Varchar2, users.Select(rf => rf.Othernames).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":gender", OracleDbType.Varchar2, users.Select(rf => rf.Gender).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":email", OracleDbType.Varchar2, users.Select(rf => rf.Email).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":username", OracleDbType.Varchar2, users.Select(rf => rf.Username).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":password", OracleDbType.Varchar2, users.Select(rf => rf.Password).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":roleid", OracleDbType.Int32, users.Select(rf => rf.RoleId).ToArray(), ParameterDirection.Input);
                command.Parameters.Add(":createdon", OracleDbType.Date, users.Select(rf => rf.CreatedOn).ToArray(), ParameterDirection.Input);

                int result = command.ExecuteNonQuery();
                if (result == users.Count)
                    saved = true;

                return saved;
            }
        }

        public void close(OracleConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
