public class StaffGlobalState : State<BaseStaff> {
    private static StaffGlobalState m_Instance = null;
    public static StaffGlobalState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StaffGlobalState();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
    }
    public override void Execute(BaseStaff go) {
    }
    public override void Exit(BaseStaff go) {
    }

}



public class StaffIdleState : State<BaseStaff> {
    private static StaffIdleState m_Instance = null;
    public static StaffIdleState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StaffIdleState();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
        go.OnIdleStart();
    }
    public override void Execute(BaseStaff go) {
        go.OnIdleExecute();
    }
    public override void Exit(BaseStaff go) {
        go.OnIdleExit();
    }

}

public class StaffMoveState : State<BaseStaff> {
    private static StaffMoveState m_Instance = null;
    public static StaffMoveState Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StaffMoveState();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
        go.OnMoveStart();
    }
    public override void Execute(BaseStaff go) {
        go.OnMoveExecute();
    }
    public override void Exit(BaseStaff go) {
        go.OnMoveExit();
    }

}

public class StaffDoWork : State<BaseStaff> {
    private static StaffDoWork m_Instance;
    public static StaffDoWork Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new StaffDoWork();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
        go.OnDoWorkStart();
    }
    public override void Execute(BaseStaff go) {
        go.OnDoWorkExecute();
    }
    public override void Exit(BaseStaff go) {
        go.OnDoWorkEnd();
    }
}

