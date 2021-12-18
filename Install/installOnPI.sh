sudo su

apt-get update && sudo apt-get upgrade -y
curl -sSL https://get.docker.com | sh
usermod -aG docker pi
usermod -aG docker ${USER}
groups ${USER}


apt-get install libffi-dev libssl-dev -y
apt install python3-dev -y
apt-get install -y python3 python3-pip
pip3 install docker-compose
systemctl enable docker


curl -fsSL https://deb.nodesource.com/setup_14.x | bash -
apt install nodejs -y


wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet5pi/master/install.sh |  bash
exec bash
dotnet tool install --global dotnet-ef

