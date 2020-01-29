"""
This script helps automate the process of issuing releases on GitHub.
* This script requires apiKeys.py to importable by Python
* Ensure .gitignore includes: apiKeys.*
* Ideally apiKeys.py is on your system outside the repo folder
"""

import requests
import json
import apiKeys
import webbrowser

def issueRelease(user="swharden", repo="SciTIF", version="1.2.3", draft=True):

    url = f'https://api.github.com/repos/{user}/{repo}/releases'

    data = {
        "tag_name": version,
        "name": f"{repo} {version}",
        "target_commitish": "master",
        "body": "CHANGELOG CONTENTS",
        "draft": draft,
        "prerelease": False
    }

    headers = {
        "Content-Type": "application/json",
        "Accept": "*/*",
        "User-Agent": "Myapp/Gunio",
    }

    headers = {'Authorization': 'token %s' % apiKeys.GITHUB}

    r = requests.post(url, data=json.dumps(data), headers=headers)
    print(r.content)

    print("Launching web browser to release page...")
    webbrowser.open(f"https://github.com/{user}/{repo}/releases")


if __name__ == "__main__":
    print("DONE")
    issueRelease()
