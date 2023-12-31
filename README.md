# wazuh-scheduler

```
#Build container
podman build -t wazuh-scheduler:0.3 -f WazuhScheduler/Dockerfile .

#Run container Linux
podman run -d \
  --name wazuh-scheduler-container \
  -e WAZUH_BASE_URL="https://wazuh-server:55000" \
  -e WAZUH_BASIC_AUTH="XXX" \
  wazuh-scheduler:0.3
  
#Run container Windows
podman run -d ^
  --name wazuh-scheduler-container ^
  -e WAZUH_BASE_URL="https://wazuh-server:55000" ^
  -e WAZUH_BASIC_AUTH="XXX" ^
  wazuh-scheduler:0.3
  
  

```