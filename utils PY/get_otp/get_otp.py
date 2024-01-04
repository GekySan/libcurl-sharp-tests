import re

def extract_curl_options_to_enum(file_path):
    with open(file_path, 'r') as file:
        file_contents = file.read()

    pattern = r"CURLOPT\((CURLOPT_[A-Z0-9_]+), CURLOPTTYPE_([A-Z0-9_]+), ([0-9]+)\)"
    matches = re.findall(pattern, file_contents)

    # https://github.com/curl/curl/blob/68f96fc9bfce90c264d4d95b91135f3ef945eea8/include/curl/curl.h#L1066C36-L1066C36
    base_values = {
        'LONG': 0,
        'OBJECTPOINT': 10000,
        'FUNCTIONPOINT': 20000,
        'OFF_T': 30000,
        'BLOB': 40000,
        'SLISTPOINT': 10000,
        'CBPOINT': 10000,
        'STRINGPOINT': 10000,
        'VALUES': 0
    }

    enum_members = []
    for name, opt_type, value in matches:
        base = base_values.get(opt_type, 0)
        enum_value = base + int(value)
        enum_members.append("    {} = {},".format(name, enum_value))

    enum_definition = "public enum CurlOption\n{\n" + "\n".join(enum_members) + "\n}"
    return enum_definition

print(extract_curl_options_to_enum('curl.h'))