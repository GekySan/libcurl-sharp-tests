using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curly
{
    internal class DefaultData
    {
        /*
         * https://curl.se/libcurl/c/CURLOPT_URL.html
        */
        public const string URL = "https://www.gekysan.fun/kaio";

        /* 
         * PUT : https://curl.se/libcurl/c/CURLOPT_UPLOAD.html
         * POST : https://curl.se/libcurl/c/CURLOPT_POSTFIELDS.html
        */
        public const string content = "";

        /*
         * https://curl.se/libcurl/c/CURLOPT_TIMEOUT_MS.html
        */
        public const int timeoutMilliseconds = 15000;

        /*
         * https://curl.se/libcurl/c/CURLOPT_MAXREDIRS.html
        */
        public const int maxNumberOfRedirects = 8;

        /*
         * https://curl.se/libcurl/c/CURLOPT_USERAGENT.html
        */
        public const string contentType = "";
        public const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";

        /*
         * https://curl.se/libcurl/c/CURLOPT_SSL_ENABLE_ALPN.html
         * This option enables/disables ALPN in the SSL handshake (if the SSL backend libcurl is built to use supports it), which can be used to negotiate http2.
        */
        public const bool enableALPN = true;

        /*
         * https://curl.se/libcurl/c/CURLOPT_SSL_FALSESTART.html
         * This option determines whether libcurl should use false start during the TLS handshake. False start is a mode where a TLS client starts sending application data before verifying the server's Finished message, thus saving a round trip when performing a full handshake.
        */
        public const bool falseStart = true;

        /*
         * https://curl.se/libcurl/c/CURLOPT_SSL_EC_CURVES.html
         * Pass a string as parameter with a colon delimited list of (EC) algorithms. This option defines the client's key exchange algorithms in the SSL handshake (if the SSL backend libcurl is built to use supports it).
        */

        public const bool customEllipticCurveAlgorithm = true;
        public const string ellipticCurveAlgorithm = "X25519:P-256:P-384:P-521";

        /*
        https://curl.se/libcurl/c/CURLOPT_SSL_CIPHER_LIST.html

        X
        */

        public const bool customCipherList = true;
        public const string cipherList = "TLS_AES_128_GCM_SHA256,TLS_AES_256_GCM_SHA384,TLS_CHACHA20_POLY1305_SHA256,ECDHE-ECDSA-AES128-GCM-SHA256,ECDHE-RSA-AES128-GCM-SHA256,ECDHE-ECDSA-AES256-GCM-SHA384,ECDHE-RSA-AES256-GCM-SHA384,ECDHE-ECDSA-CHACHA20-POLY1305,ECDHE-RSA-CHACHA20-POLY1305,ECDHE-RSA-AES128-SHA,ECDHE-RSA-AES256-SHA,AES128-GCM-SHA256,AES256-GCM-SHA384,AES128-SHA,AES256-SHA";

        /*
         * https://curl.se/libcurl/c/CURLOPT_SSL_VERIFYPEER.html
         * Pass a long as parameter to enable or disable.
         * 
         * https://curl.se/libcurl/c/CURLOPT_CAINFO.html
         * Pass a char * to a null-terminated string naming a file holding one or more certificates to verify the peer with.
        */

        public const bool verifyPeer = true;
        public const string certificatPath = "./libcurl/curl-ca-bundle.crt";


        /*
         * https://curl.se/libcurl/c/CURLOPT_DNS_SERVERS.html
         * Pass a char * that is the list of DNS servers to be used instead of the system default. The format of the dns servers option is:
         * host[:port][,host[:port]]...
         * For example:
         * 192.168.1.100,192.168.1.101,3.4.5.6
         * The application does not have to keep the string around after setting this option.
        */

        public const bool customDNS = true;
        public const string DNS = "1.1.1.1,1.0.0.1";
    }
}
