
// THIS FILE HAS BEEN AUTOGENERATED ON 4/18/2020 8:05:59 PM BY UFE.UFEedClient.GetConsts
// ReSharper disable InconsistentNaming
namespace UFE.UFEedClient 
{
	public static class Consts
	{
		// UFE CONSTS
		public const int SESSION_FLAG_REPORT = 0;
		public const int SESSION_FLAG_TXADMIN = 1;
		public const int SESSION_FLAG_SERVICE_LOGGING = 2;
		public const int UFE_ALL_SERVICES = 0x1ffff;
		public const int FIX8_OK  = 70000;
		public const int FIX_ENCODE_ERROR  = 70001;
		public const int FIX_DECODE_ERROR  = 70002;
		public const int FIX8_EXCEPTION  = 70003;
		public const int FIX8_SERVICE_STATE_CHANGE  = 70004;
		public const int UFE_OK  = 71000;
		public const int UFE_SYSTEM_SERVICE  = 71000;
		public const int UNKNOWN_SERVICE  = 71001;
		public const int UFE_ENCODE_ERROR  = 71002;
		public const int UFE_DECODE_ERROR  = 71003;
		public const int UFE_ENQUEUE_ERROR  = 71004;
		public const int UNKNOWN_TYPE  = 71005;
		public const int NO_CMD  = 71006;
		public const int INVALID_CMD  = 71007;
		public const int MISSING_FIELDS  = 71008;
		public const int UNKNOWN_USER  = 71009;
		public const int ALREADY_LOGGED_IN  = 71010;
		public const int INVALID_PASSWORD  = 71011;
		public const int USER_INSERT_FAILURE  = 71012;
		public const int LOGIN_ACCEPTED  = 71013;
		public const int NOT_LOGGED_IN  = 71014;
		public const int LOGOFF_SUCCESSFUL  = 71015;
		public const int USER_ERASE_FAILURE  = 71016;
		public const int UFE_STOPPED  = 71017;
		public const int ACCESS_DENIED  = 71018;
		public const int NO_RECORDS  = 71019;
		public const int INVALID_DIRECTION  = 71020;
		public const int INVALID_SEQUENCE  = 71021;
		public const int USER_ADD_FAILURE  = 71022;
		public const int UNKNOWN_CONFIG  = 71023;
		public const int NO_PERSISTER  = 71024;
		public const int CONFIG_FILE_ERROR  = 71025;
		public const int PERSIST_COMMIT_ERROR  = 71026;
		public const int USER_DEL_FAILURE  = 71027;
		public const int PERSIST_READ_ERROR  = 71028;
		public const int SERVICE_NOT_ESTABLISHED  = 71029;
		public const int SERVICE_ALREADY_ESTABLISHED  = 71030;
		public const int FILE_TOO_BIG  = 71031;
		public const int UFE_SERVER_GONE  = 71032;
		public const int SESSION_NOT_READY  = 71033;
		public const int BAD_CONFIG  = 71034;
		public const int UNKNOWN_REQUEST_TOPIC  = 71035;
		public const int INVALID_FILTER_SPEC  = 71036;
		public const int UNABLE_TO_STOP  = 71037;
		public const int UNABLE_TO_START  = 71038;
		public const int RPC_PROCESSED  = 71039;
		public const int UFE_RESPONSE_CODE  = 72001;
		public const int UFE_STATUS_CODE  = 72002;
		public const int UFE_REQUEST_TOKEN  = 72003;
		public const int UFE_RESPONSE_TOKEN  = 72004;
		public const int UFE_SESSION_TOKEN  = 72005;
		public const int UFE_LOGIN_ID  = 72006;
		public const int UFE_LOGIN_PW  = 72007;
		public const int UFE_LOGIN_NEWPW  = 72008;
		public const int UFE_CMD  = 72009;
		public const int UFE_CMD_RESPONSE  = 72010;
		public const int UFE_SERVICE_RECORDS  = 72011;
		public const int UFE_SERVICE_NAME  = 72012;
		public const int UFE_SERVICE_STATUS  = 72013;
		public const int UFE_SERVICE_PREV_STATUS  = 72014;
		public const int UFE_SERVICE_UPTIME  = 72015;
		public const int UFE_SERVICE_VERSION  = 72016;
		public const int UFE_SERVICE_SENT  = 72017;
		public const int UFE_SERVICE_RECEIVED  = 72018;
		public const int UFE_SERVICE_ID  = 72019;
		public const int UFE_SERVICE_TAG  = 72020;
		public const int UFE_SERVICE_PREV_STATUS_STRING  = 72021;
		public const int UFE_SERVICE_STATUS_STRING  = 72022;
		public const int UFE_SERVICE_FIX_VARIANT  = 72023;
		public const int UFE_SERVICE_FIX_DESC  = 72024;
		public const int UFE_SERVICE_STATUS_UPDATE  = 72025;
		public const int UFE_TOTAL_SENT  = 72026;
		public const int UFE_TOTAL_RECEIVED  = 72027;
		public const int UFE_RESP_SEQ  = 72028;
		public const int UFE_RECV_SEQ  = 72029;
		public const int UFE_BRD_SEQ  = 72030;
		public const int UFE_WORKERS  = 72031;
		public const int UFE_INSTANCE_NAME  = 72032;
		public const int UFE_CONF_DIR  = 72033;
		public const int UFE_PW_HASH  = 72034;
		public const int FIX8_EXCEPTION_TYPE  = 72035;
		public const int UFE_FIELD_DEFINITION_RECORDS  = 72036;
		public const int UFE_FIELD_REALM_RECORDS  = 72037;
		public const int UFE_MESSAGE_DEFINITION_RECORDS  = 72038;
		public const int UFE_COMPONENT_DEFINITION_RECORDS  = 72039;
		public const int UFE_MESSAGE_FIELD_RECORDS  = 72040;
		public const int UFE_FIX8_UNDERLYING_TYPE  = 72041;
		public const int UFE_FIX8_TYPE_STRING  = 72042;
		public const int UFE_FIX8_UNDERLYING_TYPE_STRING  = 72043;
		public const int UFE_FIX8_FLAG  = 72044;
		public const int UFE_FIX8_MESSAGE  = 72045;
		public const int UFE_FIX8_TAG  = 72046;
		public const int UFE_FIX8_TAG_TYPE  = 72047;
		public const int UFE_FIX8_TAG_STRING  = 72048;
		public const int UFE_FIX8_COMPONENT  = 72049;
		public const int UFE_FIX8_COMPONENT_STRING  = 72050;
		public const int UFE_SYSTEM_STRINGS  = 72051;
		public const int UFE_SYSTEM_STRING  = 72052;
		public const int UFE_SYSTEM_STRING_TAG  = 72053;
		public const int UFE_THREAD_ID  = 72054;
		public const int UFE_FIELD_REALM_VALUE  = 72055;
		public const int UFE_FIELD_REALM_DESCRIPTION  = 72056;
		public const int UFE_RECORD_COUNT  = 72057;
		public const int UFE_CACHE_DIRECTION  = 72058;
		public const int UFE_CACHE_SEQUENCE_BEGIN  = 72059;
		public const int UFE_CACHE_SEQUENCE_END  = 72060;
		public const int UFE_CACHE_MESSAGES  = 72061;
		public const int UFE_USER_SERVICE_PERMS  = 72062;
		public const int UFE_USER_RECORDS  = 72063;
		public const int UFE_CONFIG_NAME  = 72064;
		public const int UFE_CONFIG_RECORD  = 72065;
		public const int UFE_ILLEGAL_CONFIG  = 72066;
		public const int UFE_SESSION_FLAGS  = 72067;
		public const int UFE_NEXT_FIX_RECV_SEQ  = 72068;
		public const int UFE_NEXT_FIX_SEND_SEQ  = 72069;
		public const int UFE_LOG_TID  = 72070;
		public const int UFE_LOG_STRING  = 72071;
		public const int UFE_LOG_LEVEL  = 72072;
		public const int UFE_LOG_LINE  = 72073;
		public const int UFE_LOG_VAL  = 72074;
		public const int UFE_LOG_FILTER  = 72075;
		public const int UFE_LOG_WHEN  = 72076;
		public const int UFE_LAST_FIX_RECV_TIME  = 72077;
		public const int UFE_LAST_FIX_SEND_TIME  = 72078;
		public const int UFE_CONNECT_ATTEMPTS  = 72079;
		public const int UFE_SUBSERVICE_ID  = 72080;
		public const int UFE_LOGGED_IN  = 72081;
		public const int UFE_INITIATOR_FUNCTION_ONLY  = 72082;
		public const int UFE_IS_ACCEPTOR  = 72083;
		public const int UFE_OVERRATE  = 72084;
		public const int UFE_CONFIG_USER_TAG  = 72085;
		public const int UFE_CONFIG_MINCFG_TAG  = 72086;
		public const int UFE_CONFIG_MAINCFG_TAG  = 72087;
		public const int UFE_CONFIG_SESSION_TAG  = 72088;
		public const int UFE_CONFIG_OTHER_TAG  = 72089;
		public const int UFE_CONFIG_DESCRIPTION  = 72090;
		public const int UFE_CONFIG_RECORDS  = 72091;
		public const int UFE_CONFIG_ID  = 72092;
		public const int UFE_FIX8PRO_VERSION  = 72093;
		public const int UFE_CPU_PERCENT  = 72094;
		public const int UFE_ACTIVE_SESSIONS  = 72095;
		public const int UFE_TOTAL_SESSIONS  = 72096;
		public const int UFE_USER_VIEWONLY  = 72097;
		public const int UFE_SERVICE_BYTES_SENT  = 72098;
		public const int UFE_SERVICE_BYTES_RECEIVED  = 72099;
		public const int UFE_UPTIME  = 72100;
		public const int UFE_CONFIG_COMMIT_ACTION  = 72101;
		public const int UFE_LAST_BRD_TIME  = 72102;
		public const int UFE_HEARTBEAT_COUNT  = 72103;
		public const int UFE_SERVICE_UPDATE  = 72104;
		public const int UFE_FIX8_FLAG_REQUIRED = 0;
		public const int UFE_FIX8_FLAG_GROUP = 1;
		public const int UFE_CACHE_DIRECTION_NOTCACHED = 0;
		public const int UFE_CACHE_DIRECTION_INBOUND = 1;
		public const int UFE_CACHE_DIRECTION_OUTBOUND = 2;
		public const int UFE_CONFIG_COMMIT_ONLY = 0;
		public const int UFE_CONFIG_REPLACE_ONLY = 1;
		public const int UFE_CONFIG_COMMIT_REPLACE_LOAD = 2;
		public const int UFE_SERVICE_UPDATE_REPORT = 0;
		public const int UFE_SERVICE_UPDATE_CREATED = 1;
		public const int UFE_SERVICE_UPDATE_DELETED = 2;
		public const int UFE_CMD_LOGIN  = 73001;
		public const int UFE_CMD_LOGOUT  = 73002;
		public const int UFE_CMD_CHANGEPW  = 73003;
		public const int UFE_CMD_CHKPERM  = 73004;
		public const int UFE_CMD_SHUTDOWN  = 73005;
		public const int UFE_CMD_SERVICE_LIST  = 73006;
		public const int UFE_CMD_SERVICE_STOP  = 73007;
		public const int UFE_CMD_SERVICE_START  = 73008;
		public const int UFE_CMD_SERVICE_RESTART  = 73009;
		public const int UFE_CMD_SYSTEM_STATUS  = 73010;
		public const int UFE_CMD_KILL  = 73011;
		public const int UFE_CMD_DICTIONARY  = 73012;
		public const int UFE_CMD_SYSTEM_STRINGS  = 73013;
		public const int UFE_CMD_SESSION_CACHE  = 73014;
		public const int UFE_CMD_ADD_USER  = 73015;
		public const int UFE_CMD_UPDATE_USER  = 73016;
		public const int UFE_CMD_REMOVE_USER  = 73017;
		public const int UFE_CMD_GET_USERS  = 73018;
		public const int UFE_CMD_GET_CONFIG  = 73019;
		public const int UFE_CMD_PUT_CONFIG  = 73020;
		public const int UFE_CMD_REMOVE_CONFIG  = 73021;
		public const int UFE_CMD_RESTART  = 73022;
		public const int UFE_CMD_SET_SESSION_FLAGS  = 73023;
		public const int UFE_CMD_GET_SESSION_FLAGS  = 73024;
		public const int UFE_CMD_GET_SEND_RECV  = 73025;
		public const int UFE_CMD_GET_CONFIG_LIST  = 73026;
		public const int UFE_CMD_INTERNAL_REPORT  = 73027;
		public const int UFE_CMD_AUTHENTICATE  = 73028;
		public const int UFE_CMD_LOAD_PROFILE  = 73029;
		public const int UFE_CMD_SERVICE_STATUS  = 73030;
		public const int UFE_CMD_RPC  = 73031;
		public const int UFE_COMMON_FIX = 80000;

		public const int UFE_FLOAT_PRECISION = 2;

		// UFEGW CONSTS
		public const string SUBSCRIBER = "subscriber";
		public const string SUBSCRIBER_DEFAULT = "tcp://127.0.0.1:55745";
		public const string REQUESTER = "requester";
		public const string REQUESTER_DEFAULT = "tcp://127.0.0.1:55746";
		public const string PUBLISHER = "publisher";
		public const string PUBLISHER_DEFAULT = "tcp://*:55747";
		public const string RESPONDER = "responder";
		public const string RESPONDER_DEFAULT = "tcp://*:55748";
		public const string SUBSCRIBER_TOPIC = "subscribertopic";
		public const string SUBSCRIBER_TOPIC_DEFAULT = "ufegw-publisher";
		public const string REQUESTER_TOPIC = "requestertopic";
		public const string REQUESTER_TOPIC_DEFAULT = "ufegw-responder";
		public const string PUBLISHER_TOPIC = "publishertopic";
		public const string PUBLISHER_TOPIC_DEFAULT = "ufeedclient-publisher";
		public const string RESPONDER_TOPIC = "respondertopic";
		public const string RESPONDER_TOPIC_DEFAULT = "ufeedclient-responder";            

		// FIX CONSTS
		public const string COMMON_MSGTYPE_HEARTBEAT = "0";
		public const string COMMON_MSGTYPE_TEST_REQUEST = "1";
		public const string COMMON_MSGTYPE_RESEND_REQUEST = "2";
		public const string COMMON_MSGTYPE_REJECT = "3";
		public const string COMMON_MSGTYPE_SEQUENCE_RESET = "4";
		public const string COMMON_MSGTYPE_LOGOUT = "5";
		public const string COMMON_MSGTYPE_LOGON = "A";
		public const string COMMON_MSGTYPE_BUSINESS_REJECT = "j";
		public const char COMMON_MSGBYTE_HEARTBEAT = '0';
		public const char COMMON_MSGBYTE_TEST_REQUEST = '1';
		public const char COMMON_MSGBYTE_RESEND_REQUEST = '2';
		public const char COMMON_MSGBYTE_REJECT = '3';
		public const char COMMON_MSGBYTE_SEQUENCE_RESET = '4';
		public const char COMMON_MSGBYTE_LOGOUT = '5';
		public const char COMMON_MSGBYTE_LOGON = 'A';
		public const char COMMON_MSGBYTE_BUSINESS_REJECT = 'j';
		public const string COMMON_MSGTYPE_EXECUTION_REPORT = "8";
		public const string COMMON_MSGTYPE_CONFIRMATION = "AK";
		public const string COMMON_MSGTYPE_MASSORDERACK = "DK";
		public const string COMMON_MSGTYPE_PARTY_ACTION_REPORT = "DI";
		public const string COMMON_MSGTYPE_TRADE_CAPTURE_REPORT = "AE";
		public const string COMMON_MSGTYPE_TRADE_CAPTURE_REPORT_ACK = "AR";
		public const int COMMON_ACCOUNT = 1;
		public const int COMMON_AVGPX = 6;
		public const int COMMON_BEGINSEQNO = 7;
		public const int COMMON_BEGINSTRING = 8;
		public const int COMMON_BODYLENGTH = 9;
		public const int COMMON_CHECKSUM = 10;
		public const int COMMON_CLORDID = 11;
		public const int COMMON_CUMQTY = 14;
		public const int COMMON_ENDSEQNO = 16;
		public const int COMMON_EXECID = 17;
		public const int COMMON_EXECTRANSTYPE = 20;
		public const int COMMON_HANDLINST = 21;
		public const int COMMON_LASTPX = 34;
		public const int COMMON_MSGSEQNUM = 34;
		public const int COMMON_MSGTYPE = 35;
		public const int COMMON_NEWSEQNO = 36;
		public const int COMMON_ORDERID = 37;
		public const int COMMON_ORDERQTY = 38;
		public const int COMMON_ORDSTATUS = 39;
		public const int COMMON_ORDTYPE = 40;
		public const int COMMON_ORIGCLORDID = 41;
		public const int COMMON_POSSDUPFLAG = 43;
		public const int COMMON_PRICE = 44;
		public const int COMMON_REFSEQNUM = 45;
		public const int COMMON_SENDERCOMPID = 49;
		public const int COMMON_SENDINGTIME = 52;
		public const int COMMON_SIDE = 54;
		public const int COMMON_SYMBOL = 55;
		public const int COMMON_TARGETCOMPID = 56;
		public const int COMMON_TEXT = 58;
		public const int COMMON_TIMEINFORCE = 59;
		public const int COMMON_TRANSACTTIME = 60;
		public const int COMMON_SECUREDATALEN = 90;
		public const int COMMON_RAWDATALEN = 95;
		public const int COMMON_ENCRYPTMETHOD = 98;
		public const int COMMON_HEARTBTINT = 108;
		public const int COMMON_TESTREQID = 112;
		public const int COMMON_ONBEHALFOFCOMPID = 115;
		public const int COMMON_ONBEHALFOFSUBID = 116;
		public const int COMMON_ORIGSENDINGTIME = 122;
		public const int COMMON_GAPFILLFLAG = 123;
		public const int COMMON_DELIVERTOCOMPID = 128;
		public const int COMMON_RESETSEQNUMFLAG = 141;
		public const int COMMON_ONBEHALFOFLOCATIONID = 144;
		public const int COMMON_EXECTYPE = 150;
		public const int COMMON_LEAVESQTY = 151;
		public const int COMMON_XMLDATALEN = 212;
		public const int COMMON_ONBEHALFOFSENDINGTIME = 370;
		public const int COMMON_REFMSGTYPE = 372;
		public const int COMMON_MAXMESSAGESIZE = 383;
		public const int COMMON_BUSINESSREJECTREASON = 380;
		public const int COMMON_TESTMESSAGEINDICATOR = 464;
		public const int COMMON_NOHOPS = 627;
		public const int COMMON_HOPCOMPID = 628;
		public const int COMMON_HOPSENDINGTIME = 629;
		public const int COMMON_HOPREFID = 630;
		public const int COMMON_NEXTEXPECTEDMSGSEQNUM = 789;
		public const int COMMON_COPYMSGINDICATOR = 797;
		public const int COMMON_DEFAULTAPPLVERID = 1137;
		public const int COMMON_BUSINESSREJECTREASON_OTHER = 0;
		public const int COMMON_BUSINESSREJECTREASON_UNKOWN_ID = 1;
		public const int COMMON_BUSINESSREJECTREASON_UNKNOWN_SECURITY = 2;
		public const int COMMON_BUSINESSREJECTREASON_UNSUPPORTED_MESSAGE_TYPE = 3;
		public const int COMMON_BUSINESSREJECTREASON_APPLICATION_NOT_AVAILABLE = 4;
		public const int COMMON_BUSINESSREJECTREASON_CONDITIONALLY_REQUIRED_FIELD_MISSING = 5;
		public const int COMMON_APPLVERID_FIX27 = 0;
		public const int COMMON_APPLVERID_FIX30 = 1;
		public const int COMMON_APPLVERID_FIX40 = 2;
		public const int COMMON_APPLVERID_FIX41 = 3;
		public const int COMMON_APPLVERID_FIX42 = 4;
		public const int COMMON_APPLVERID_FIX43 = 5;
		public const int COMMON_APPLVERID_FIX44 = 6;
		public const int COMMON_APPLVERID_FIX50 = 7;
		public const int COMMON_APPLVERID_FIX50SP1 = 8;
		public const int COMMON_APPLVERID_FIX50SP2 = 9;
		public const int COMMON_VERSION_4_DOT_0 = 4000;
		public const int COMMON_VERSION_4_DOT_1 = 4100;
		public const int COMMON_VERSION_4_DOT_2 = 4200;
		public const int COMMON_VERSION_4_DOT_3 = 4300;
		public const int COMMON_VERSION_4_DOT_4 = 4400;
		public const int COMMON_VERSION_FIXT_1_DOT_1 = 1100;

	}
}

