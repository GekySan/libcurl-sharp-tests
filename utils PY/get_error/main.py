import re

with open("./errors.html", 'r') as file:
        file_contents = file.read()

pattern = r"<span class=\"nroffip\">(CURLE_[A-Z_]+) \(([0-9]+)\)</span> </p>\s*<p class=\"level1\">(.*?) </p>"
matches = re.findall(pattern, file_contents, re.DOTALL)

error_messages = {int(code): f"{name}: {message}" for name, code, message in matches}

csharp_dict = "public static readonly Dictionary<int, string> ErrorMessages = new Dictionary<int, string>\n{\n"
for code, message in error_messages.items():
    short_message = message.split(':')[1].strip().split('.')[0]
    short_message = short_message.replace('"', '\\"')

    csharp_dict += f"    {{{code}, \"{short_message}.\"}},\n"
csharp_dict += "};"

print(csharp_dict)