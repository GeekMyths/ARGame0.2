namespace API
{
	public class Constant
	{
		public static string host = "115.28.140.76";
		public static int port = 8080;
		public static string ip = "http://115.28.140.76:8080/Game1/s/";
		public static string login = ip + "user/login";
		public static string logout = ip + "user/logout";
		public static string register = ip + "user/register";
		public static string inviteEnemy = ip + "battle/inviteEnemy";
		public static string handleMessage = ip + "battle/handleMessage";
		public static string getMyEmp = ip + "battle/getMyEmp";


		public static string wsip = "ws://115.28.140.76:8080/Game1/s/";
		public static string wsonline = wsip+"webSocketServer";
	}
}
