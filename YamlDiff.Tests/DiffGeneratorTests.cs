using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class DiffGeneratorTests
    {
        [Fact]
        public void IdenticalSinglePropertyDocumentsMatch()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem: ipsum
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));

            Assert.Empty(result.Entries);
        }

        [Fact]
        public void DetectChangedMappingNodeScalarNode()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem: dolor
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));

            Assert.Single(result.Entries);
            Assert.Single(result.Entries.Single().Path.Segments);
            Assert.Equal("lorem", result.Entries.Single().Path.Segments.Single());
        }

        [Fact]
        public void DetectChangedNestedMappingNodeScalarNode()
        {
            var originalDocument = @"
                lorem:
                    ipsum: dolor
            ";
            var changedDocument = @"
                lorem:
                    ipsum: sit
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));

            Assert.Single(result.Entries);
            Assert.Equal(2, result.Entries.Single().Path.Segments.Count());
            Assert.Equal("lorem", result.Entries.Single().Path.Segments.First());
            Assert.Equal("ipsum", result.Entries.Single().Path.Segments.Last());
        }

        [Fact]
        public void DetectMappingNodeHavingScalarNodeChangedToMappingNode()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem:
                    ipsum: sit
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));

            Assert.Single(result.Entries);
            Assert.Single(result.Entries.Single().Path.Segments);
            Assert.Equal("lorem", result.Entries.Single().Path.Segments.Single());
        }

        [Fact]
        public void DetectMissingMappingNodeScalarNodeInB()
        {
            var originalDocument = @"
                lorem: ipsum
                dolor: sit
            ";
            var changedDocument = @"
                dolor: sit
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));

            Assert.Single(result.Entries);
            Assert.Single(result.Entries.Single().Path.Segments);
            Assert.Equal("lorem", result.Entries.Single().Path.Segments.Single());
        }

        [Fact]
        public void LiveYamlFiles()
        {
            var originalDocument = @"
kind: Deployment
apiVersion: extensions/v1beta1
metadata:
  name: email-service
spec:
  replicas: 1
  strategy:
    rollingUpdate:
      maxUnavailable: 5%
      maxSurge: 1
  template:
    metadata:
      labels:
        app: email-service
        tier: backend
        stack: dotnet
    spec:
      imagePullSecrets:
      - name: docker-hub-key
      containers:
      - name: email-service
        image: nobia/email-service:latest
        imagePullPolicy: Always
        livenessProbe:
          httpGet:
            path: /api/healthcheck
            port: 80
          initialDelaySeconds: 20
        readinessProbe:
          httpGet:
            path: /api/healthcheck
            port: 80
          initialDelaySeconds: 20
        env:
        - name: ServiceBus_CustomersTopic
          value: customerstopic
        - name: ServiceBus_CrmIntegrationTopic
          value: crmintegrationtopic
        - name: ServiceBus_CustomersSubscription
          value: emailservicesubscription
        - name: BackendGatewayUrl
          value: https://backend-gateway-api.dev.nobiadigital.com
        - name: Authentication_Endpoint_Authorize
          value: https://nobia-dev.eu.auth0.com/authorize
        - name: Authentication_Endpoint_GenerateToken
          value: https://nobia-dev.eu.auth0.com
        - name: Authentication_ClientId
          value: b0nPqseb0q4laLE8rZRPm9LxRuqIdZ1K
        - name: Loggly_EndpointHostname
          value: logs-01.loggly.com
        - name: Loggly_ApplicationName
          value: email-service
        - name: Authentication_ManagementApiAudience
          value: https://nobia-dev.eu.auth0.com/api/v2/
        - name: ServiceBus_EmailServiceTopic
          value: emailservicetopic
        - name: Redis_InstanceName
          value: Email
        - name: Appointment_Details_Url
          value: https://marbodal.nobia.netrelations.se/appointment-details
        - name: MarketingEmailProvider
          value: customerio
        - name: MarketingEmailProvider_MARBODAL
          value: marketo
        envFrom:
        - configMapRef:
            name: dotnet
        - secretRef:
            name: service-bus-connection
        - secretRef:
            name: auth0
        - secretRef:
            name: loggly
        - secretRef:
            name: customerio
        - secretRef:
            name: emailservice
        - secretRef:
            name: redis-connection
        - secretRef:
            name: marketo
        resources:
          requests:
            cpu: 100m
            memory: 50Mi
          limits:
            cpu: 120m
            memory: 200Mi
        ports:
        - containerPort: 80
            ";
            var changedDocument = @"
kind: Deployment
apiVersion: v1
metadata:
  annotations:
    deployment.kubernetes.io/revision: ""32""
    kubectl.kubernetes.io/last-applied-configuration: |
      {""apiVersion"":""extensions/v1beta1"",""kind"":""Deployment"",""metadata"":{""annotations"":{},""name"":""email-service"",""namespace"":""default""},""spec"":{""replicas"":1,""strategy"":{""rollingUpdate"":{""maxSurge"":1,""maxUnavailable"":""5%""}},""template"":{""metadata"":{""labels"":{""app"":""email-service"",""stack"":""dotnet"",""tier"":""backend""}},""spec"":{""containers"":[{""env"":[{""name"":""ServiceBus_CustomersTopic"",""value"":""customerstopic""},{""name"":""ServiceBus_CrmIntegrationTopic"",""value"":""crmintegrationtopic""},{""name"":""ServiceBus_CustomersSubscription"",""value"":""emailservicesubscription""},{""name"":""BackendGatewayUrl"",""value"":""https://backend-gateway-api.dev.nobiadigital.com""},{""name"":""Authentication_Endpoint_Authorize"",""value"":""https://nobia-dev.eu.auth0.com/authorize""},{""name"":""Authentication_Endpoint_GenerateToken"",""value"":""https://nobia-dev.eu.auth0.com""},{""name"":""Authentication_ClientId"",""value"":""b0nPqseb0q4laLE8rZRPm9LxRuqIdZ1K""},{""name"":""Loggly_EndpointHostname"",""value"":""logs-01.loggly.com""},{""name"":""Loggly_ApplicationName"",""value"":""email-service""},{""name"":""Authentication_ManagementApiAudience"",""value"":""https://nobia-dev.eu.auth0.com/api/v2/""},{""name"":""ServiceBus_EmailServiceTopic"",""value"":""emailservicetopic""},{""name"":""Redis_InstanceName"",""value"":""Email""},{""name"":""Appointment_Details_Url"",""value"":""https://marbodal.nobia.netrelations.se/appointment-details""}],""envFrom"":[{""configMapRef"":{""name"":""dotnet""}},{""secretRef"":{""name"":""service-bus-connection""}},{""secretRef"":{""name"":""auth0""}},{""secretRef"":{""name"":""loggly""}},{""secretRef"":{""name"":""customerio""}},{""secretRef"":{""name"":""emailservice""}},{""secretRef"":{""name"":""redis-connection""}},{""secretRef"":{""name"":""marketo""}}],""image"":""nobia/email-service:latest"",""imagePullPolicy"":""Always"",""livenessProbe"":{""httpGet"":{""path"":""/api/healthcheck"",""port"":80},""initialDelaySeconds"":20},""name"":""email-service"",""ports"":[{""containerPort"":80}],""readinessProbe"":{""httpGet"":{""path"":""/api/healthcheck"",""port"":80},""initialDelaySeconds"":20},""resources"":{""limits"":{""cpu"":""120m"",""memory"":""200Mi""},""requests"":{""cpu"":""100m"",""memory"":""50Mi""}}}],""imagePullSecrets"":[{""name"":""docker-hub-key""}]}}}}
  creationTimestamp: 2018-08-29T13:01:47Z
  generation: 36
  labels:
    app: email-service
    stack: dotnet
    tier: backend
  name: email-service
  namespace: default
  resourceVersion: ""41054553""
  selfLink: /apis/extensions/v1beta1/namespaces/default/deployments/email-service
  uid: b0045769-ab8b-11e8-8893-b22ec1cb4619
spec:
  progressDeadlineSeconds: 600
  replicas: 1
  revisionHistoryLimit: 10
  selector:
    matchLabels:
      app: email-service
      stack: dotnet
      tier: backend
  strategy:
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 5%
    type: RollingUpdate
  template:
    metadata:
      creationTimestamp: null
      labels:
        app: email-service
        stack: dotnet
        tier: backend
    spec:
      containers:
      - env:
        - name: ServiceBus_CustomersTopic
          value: customerstopic
        - name: ServiceBus_CrmIntegrationTopic
          value: crmintegrationtopic
        - name: ServiceBus_CustomersSubscription
          value: emailservicesubscription
        - name: BackendGatewayUrl
          value: https://backend-gateway-api.dev.nobiadigital.com
        - name: Authentication_Endpoint_Authorize
          value: https://nobia-dev.eu.auth0.com/authorize
        - name: Authentication_Endpoint_GenerateToken
          value: https://nobia-dev.eu.auth0.com
        - name: Authentication_ClientId
          value: b0nPqseb0q4laLE8rZRPm9LxRuqIdZ1K
        - name: Loggly_EndpointHostname
          value: logs-01.loggly.com
        - name: Loggly_ApplicationName
          value: email-service
        - name: Authentication_ManagementApiAudience
          value: https://nobia-dev.eu.auth0.com/api/v2/
        - name: ServiceBus_EmailServiceTopic
          value: emailservicetopic
        - name: Redis_InstanceName
          value: Email
        - name: Appointment_Details_Url
          value: https://marbodal.nobia.netrelations.se/appointment-details
        envFrom:
        - configMapRef:
            name: dotnet
        - secretRef:
            name: service-bus-connection
        - secretRef:
            name: auth0
        - secretRef:
            name: loggly
        - secretRef:
            name: customerio
        - secretRef:
            name: emailservice
        - secretRef:
            name: redis-connection
        - secretRef:
            name: marketo
        image: nobia/email-service:feature_bada-marketo-integration-793
        imagePullPolicy: Always
        livenessProbe:
          failureThreshold: 3
          httpGet:
            path: /api/healthcheck
            port: 80
            scheme: HTTP
          initialDelaySeconds: 20
          periodSeconds: 10
          successThreshold: 1
          timeoutSeconds: 1
        name: email-service
        ports:
        - containerPort: 80
          protocol: TCP
        readinessProbe:
          failureThreshold: 3
          httpGet:
            path: /api/healthcheck
            port: 80
            scheme: HTTP
          initialDelaySeconds: 20
          periodSeconds: 10
          successThreshold: 1
          timeoutSeconds: 1
        resources:
          limits:
            cpu: 120m
            memory: 200Mi
          requests:
            cpu: 100m
            memory: 50Mi
        terminationMessagePath: /dev/termination-log
        terminationMessagePolicy: File
      dnsPolicy: ClusterFirst
      imagePullSecrets:
      - name: docker-hub-key
      restartPolicy: Always
      schedulerName: default-scheduler
      securityContext: {}
      terminationGracePeriodSeconds: 30
status:
  availableReplicas: 1
  conditions:
  - lastTransitionTime: 2019-07-18T12:32:33Z
    lastUpdateTime: 2019-07-18T12:32:33Z
    message: Deployment has minimum availability.
    reason: MinimumReplicasAvailable
    status: ""True""
    type: Available
  - lastTransitionTime: 2018-08-29T13:01:47Z
    lastUpdateTime: 2019-07-18T13:47:23Z
    message: ReplicaSet ""email-service-846f5f88cc"" has successfully progressed.
    reason: NewReplicaSetAvailable
    status: ""True""
    type: Progressing
  observedGeneration: 36
  readyReplicas: 1
  replicas: 1
  updatedReplicas: 1
            ";

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(originalDocument), parser.Parse(changedDocument));
        }
    }
}
