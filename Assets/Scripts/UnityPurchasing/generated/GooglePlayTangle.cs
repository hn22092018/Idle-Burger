// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ZYoJOnlXurZHJsxxH7jcm2H9mbhSn4romN3LukmS/gov6vxHE9AQNvMzuxRkQcKEoIiPImZ5bTIVclk0QsvBGlTMYEvtGlUbSvDNG13n3NZJJjQzifjVaVwA7N0rC+GRSZGbxhgmFTX9vy+rEyM8BHTkaHgBcJn5fdfiFK3sNrbqEPcOHjd6ZeCZ1KHuaaODPX2Ab1JxJeIL5YbvDExPbWvZWnlrVl1Scd0T3axWWlpaXltYyJ4tl+brAQx4wY5f9hVCOWCC+dzpY6uDn0enm0dnXOyKmCk59oNhxqdcMP1KMGeVmXcTVBiFKt/LGUbnAXuCrDI/WcoZ8JWgU0iRvwSo3jfZWlRba9laUVnZWlpb3rw6/Kp1Bno9yLNccsd8YllYWlta");
        private static int[] order = new int[] { 2,4,3,8,11,6,9,10,8,9,10,13,12,13,14 };
        private static int key = 91;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
