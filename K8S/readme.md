## RUN Commands

Start the deployment
```
kubectl apply -f platforms-depl.yaml 
```

Check running deployments and pods
```kubectl get deployments
kubectl get pods
```

Delete Deployment
```
kubectl delete deployment platforms-depl
```

Start a service
-- You have to start the deployment and then the service 
```
kubectl apply -f platforms-depl.yaml
kubectl apply -f platforms-np-srv.yaml  
```

Get the services
```
kubectl get services
```
Check the port allocated by the NodePort created, the api should be available on that port

Delete Service -- remember to delete the deployment after
```
kubectl delete svc platformnpservice-srv 
```
