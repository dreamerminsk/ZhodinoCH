using System;
using System.Diagnostics;

namespace ZhodinoCH.Utils
{
    public static class PushIDGenerator
    {

        private static readonly Random random = new Random();

        private const string PUSH_CHARS = "-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";

        public static string GeneratePushId()
        {
            long lastPushTime = 0L;

            char[] lastRandChars = new char[72];

            long now = (DateTime.Now.Ticks - 621355968000000000L) / 10000;

            bool duplicateTime = now == lastPushTime;

            char[] timeStampChars = new char[8];
            for (int i = 7; i >= 0; i--)
            {
                long module = now % 64;
                timeStampChars[i] = PUSH_CHARS[(int)module];
                now = (long)Math.Floor((decimal)now / 64);
            }

            Debug.Assert(now == 0, "We should have converted the entire timestamp.\r\n" + now);

            String id = new string(timeStampChars);

            if (!duplicateTime)
            {
                for (int i = 0; i < 12; i++)
                {
                    int times = random.Next(64);
                    lastRandChars[i] = (char)times;

                }
            }
            else
            {
                int lastValueOfInt = 0;
                for (int i = 11; i >= 0 && lastRandChars[i] == 63; i--)
                {
                    lastValueOfInt = i;
                    lastRandChars[i] = (char)0;
                }
                lastRandChars[lastValueOfInt]++;
            }

            for (int i = 0; i < 12; i++)
            {
                id += PUSH_CHARS[lastRandChars[i]];
            }

            Debug.Assert(id.Length == 20, "Length should be 20.");

            return id;
        }

    }
}
