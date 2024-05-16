namespace ThirdPartyFreight.Application.Approvals.AddApproval;


public class ElsaWorkFlowResponse
{
    public WorkflowState workflowState { get; set; }
}

public class WorkflowState
{
    public string id { get; set; }
    public Bookmarks[] bookmarks { get; set; }
}

public class Bookmarks
{
    public Payload payload { get; set; }
}

public class Payload
{
    public string taskId { get; set; }
    public string taskName { get; set; }
}
