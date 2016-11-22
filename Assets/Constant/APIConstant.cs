namespace API
{
	public class Constant
	{
		public static string host = "220.184.61.78";
		public static string ip = "http://"+host+":8080/Game1/m/";
		public static string login = ip + "user/login";
		public static string logout = ip + "user/logout";
		public static string register = ip + "user/register";
		public static string inviteEnemy = ip + "battle/invite";
		public static string getMyEmp = ip + "employee/my";
		public static string friend=ip +"friend/invite";
		public static string messages = ip + "/messages";
		public static string acceptMessage = ip + "/message/accept";
		public static string denyMessage = ip + "/message/deny";
		public static string cancelMessage = ip + "/message/cancel";
		public static string finishMessage = ip + "/message/finish";


		public static string wsip = "ws://"+host+":8080/Game1/";
		public static string wsonline = wsip+"webSocketServer";
	}
}
