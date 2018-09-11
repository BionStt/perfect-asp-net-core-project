#!/bin/bash
deployTo = $1
for f in `curl ftp://$FTP_HOST:$FTP_DEPLOYER_USERNAME@FTP_DEPLOYER_PASSWORD $deployTo/`; do

  # Delete each file individually
  curl ftp://$FTP_HOST:$FTP_DEPLOYER_USERNAME@FTP_DEPLOYER_PASSWORD -Q "DELE $deployTo/$f"
done
find release -type f -exec curl -u $FTP_DEPLOYER_USERNAME:FTP_DEPLOYER_PASSWORD --ftp-create-dirs -T {} ftp://$FTP_HOST/$deployTo/{} \;