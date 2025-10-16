#!/bin/bash
set -euo pipefail

LOGFILE=/srv/GymTrack/deploy_gymtrack.log
echo "Deploy start" >> "$LOGFILE"

cd /srv/GymTrack || { echo "folder /srv/GymTrack nie istnieje"; exit 1; }

echo "Deploy stop container" >> "$LOGFILE"
docker-compose down

echo "Deploy pull" >> "$LOGFILE"
git pull origin main

echo "Deploy build" >> "$LOGFILE"
docker-compose build

echo "Deploy start" >> "$LOGFILE"
docker-compose up -d

echo "Deploy finished" >> "$LOGFILE"