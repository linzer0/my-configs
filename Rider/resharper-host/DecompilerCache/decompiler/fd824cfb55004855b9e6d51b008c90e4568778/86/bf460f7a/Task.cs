// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.Task
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FD824CFB-5500-4855-B9E6-D51B008C90E4
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
  [DebuggerTypeProxy(typeof (SystemThreadingTasks_TaskDebugView))]
  [DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}")]
  [__DynamicallyInvokable]
  [HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
  public class Task : IThreadPoolWorkItem, IAsyncResult, IDisposable
  {
    [ThreadStatic]
    internal static Task t_currentTask;
    [ThreadStatic]
    private static StackGuard t_stackGuard;
    internal static int s_taskIdCounter;
    private static readonly TaskFactory s_factory = new TaskFactory();
    private volatile int m_taskId;
    internal object m_action;
    internal object m_stateObject;
    internal TaskScheduler m_taskScheduler;
    internal readonly Task m_parent;
    internal volatile int m_stateFlags;
    private const int OptionsMask = 65535;
    internal const int TASK_STATE_STARTED = 65536;
    internal const int TASK_STATE_DELEGATE_INVOKED = 131072;
    internal const int TASK_STATE_DISPOSED = 262144;
    internal const int TASK_STATE_EXCEPTIONOBSERVEDBYPARENT = 524288;
    internal const int TASK_STATE_CANCELLATIONACKNOWLEDGED = 1048576;
    internal const int TASK_STATE_FAULTED = 2097152;
    internal const int TASK_STATE_CANCELED = 4194304;
    internal const int TASK_STATE_WAITING_ON_CHILDREN = 8388608;
    internal const int TASK_STATE_RAN_TO_COMPLETION = 16777216;
    internal const int TASK_STATE_WAITINGFORACTIVATION = 33554432;
    internal const int TASK_STATE_COMPLETION_RESERVED = 67108864;
    internal const int TASK_STATE_THREAD_WAS_ABORTED = 134217728;
    internal const int TASK_STATE_WAIT_COMPLETION_NOTIFICATION = 268435456;
    internal const int TASK_STATE_EXECUTIONCONTEXT_IS_NULL = 536870912;
    internal const int TASK_STATE_TASKSCHEDULED_WAS_FIRED = 1073741824;
    private const int TASK_STATE_COMPLETED_MASK = 23068672;
    private const int CANCELLATION_REQUESTED = 1;
    private volatile object m_continuationObject;
    private static readonly object s_taskCompletionSentinel = new object();
    [FriendAccessAllowed]
    internal static bool s_asyncDebuggingEnabled;
    private static readonly Dictionary<int, Task> s_currentActiveTasks = new Dictionary<int, Task>();
    private static readonly object s_activeTasksLock = new object();
    internal volatile Task.ContingentProperties m_contingentProperties;
    private static readonly Action<object> s_taskCancelCallback = new Action<object>(Task.TaskCancelCallback);
    private static readonly Func<Task.ContingentProperties> s_createContingentProperties = (Func<Task.ContingentProperties>) (() => new Task.ContingentProperties());
    private static Task s_completedTask;
    private static readonly Predicate<Task> s_IsExceptionObservedByParentPredicate = (Predicate<Task>) (t => t.IsExceptionObservedByParent);
    [SecurityCritical]
    private static ContextCallback s_ecCallback;
    private static readonly Predicate<object> s_IsTaskContinuationNullPredicate = (Predicate<object>) (tc => tc == null);

    [FriendAccessAllowed]
    internal static bool AddToActiveTasks(Task task)
    {
      lock (Task.s_activeTasksLock)
        Task.s_currentActiveTasks[task.Id] = task;
      return true;
    }

    [FriendAccessAllowed]
    internal static void RemoveFromActiveTasks(int taskId)
    {
      lock (Task.s_activeTasksLock)
        Task.s_currentActiveTasks.Remove(taskId);
    }

    internal Task(bool canceled, TaskCreationOptions creationOptions, CancellationToken ct)
    {
      int num = (int) creationOptions;
      if (canceled)
      {
        this.m_stateFlags = 5242880 | num;
        Task.ContingentProperties contingentProperties;
        this.m_contingentProperties = contingentProperties = new Task.ContingentProperties();
        contingentProperties.m_cancellationToken = ct;
        contingentProperties.m_internalCancellationRequested = 1;
      }
      else
        this.m_stateFlags = 16777216 | num;
    }

    internal Task() => this.m_stateFlags = 33555456;

    internal Task(object state, TaskCreationOptions creationOptions, bool promiseStyle)
    {
      if ((creationOptions & ~(TaskCreationOptions.AttachedToParent | TaskCreationOptions.RunContinuationsAsynchronously)) != TaskCreationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (creationOptions));
      if ((creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None)
        this.m_parent = Task.InternalCurrent;
      this.TaskConstructorCore((object) null, state, new CancellationToken(), creationOptions, InternalTaskOptions.PromiseTask, (TaskScheduler) null);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action action)
      : this((Delegate) action, (object) null, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action action, CancellationToken cancellationToken)
      : this((Delegate) action, (object) null, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action action, TaskCreationOptions creationOptions)
      : this((Delegate) action, (object) null, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(
      Action action,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this((Delegate) action, (object) null, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action<object> action, object state)
      : this((Delegate) action, state, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action<object> action, object state, CancellationToken cancellationToken)
      : this((Delegate) action, state, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(Action<object> action, object state, TaskCreationOptions creationOptions)
      : this((Delegate) action, state, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task(
      Action<object> action,
      object state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this((Delegate) action, state, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      this.PossiblyCaptureContext(ref stackMark);
    }

    internal Task(
      Action<object> action,
      object state,
      Task parent,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler,
      ref StackCrawlMark stackMark)
      : this((Delegate) action, state, parent, cancellationToken, creationOptions, internalOptions, scheduler)
    {
      this.PossiblyCaptureContext(ref stackMark);
    }

    internal Task(
      Delegate action,
      object state,
      Task parent,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      if ((object) action == null)
        throw new ArgumentNullException(nameof (action));
      if ((creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None || (internalOptions & InternalTaskOptions.SelfReplicating) != InternalTaskOptions.None)
        this.m_parent = parent;
      this.TaskConstructorCore((object) action, state, cancellationToken, creationOptions, internalOptions, scheduler);
    }

    internal void TaskConstructorCore(
      object action,
      object state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      this.m_action = action;
      this.m_stateObject = state;
      this.m_taskScheduler = scheduler;
      if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously)) != TaskCreationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (creationOptions));
      if ((creationOptions & TaskCreationOptions.LongRunning) != TaskCreationOptions.None && (internalOptions & InternalTaskOptions.SelfReplicating) != InternalTaskOptions.None)
        throw new InvalidOperationException(Environment.GetResourceString("Task_ctor_LRandSR"));
      int num = (int) (creationOptions | (TaskCreationOptions) internalOptions);
      if (this.m_action == null || (internalOptions & InternalTaskOptions.ContinuationTask) != InternalTaskOptions.None)
        num |= 33554432;
      this.m_stateFlags = num;
      if (this.m_parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None && (this.m_parent.CreationOptions & TaskCreationOptions.DenyChildAttach) == TaskCreationOptions.None)
        this.m_parent.AddNewChild();
      if (!cancellationToken.CanBeCanceled)
        return;
      this.AssignCancellationToken(cancellationToken, (Task) null, (TaskContinuation) null);
    }

    private void AssignCancellationToken(
      CancellationToken cancellationToken,
      Task antecedent,
      TaskContinuation continuation)
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized(false);
      contingentProperties.m_cancellationToken = cancellationToken;
      try
      {
        if (AppContextSwitches.ThrowExceptionIfDisposedCancellationTokenSource)
          cancellationToken.ThrowIfSourceDisposed();
        if ((this.Options & (TaskCreationOptions) 13312) != TaskCreationOptions.None)
          return;
        if (cancellationToken.IsCancellationRequested)
        {
          this.InternalCancel(false);
        }
        else
        {
          CancellationTokenRegistration tokenRegistration = antecedent != null ? cancellationToken.InternalRegisterWithoutEC(Task.s_taskCancelCallback, (object) new Tuple<Task, Task, TaskContinuation>(this, antecedent, continuation)) : cancellationToken.InternalRegisterWithoutEC(Task.s_taskCancelCallback, (object) this);
          contingentProperties.m_cancellationRegistration = new Shared<CancellationTokenRegistration>(tokenRegistration);
        }
      }
      catch
      {
        if (this.m_parent != null && (this.Options & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None && (this.m_parent.Options & TaskCreationOptions.DenyChildAttach) == TaskCreationOptions.None)
          this.m_parent.DisregardChild();
        throw;
      }
    }

    private static void TaskCancelCallback(object o)
    {
      switch (o)
      {
        case Tuple<Task, Task, TaskContinuation> tuple:
          pattern_0 = tuple.Item1;
          tuple.Item2.RemoveContinuation((object) tuple.Item3);
          break;
      }
      pattern_0.InternalCancel(false);
    }

    private string DebuggerDisplayMethodDescription
    {
      get
      {
        Delegate action = (Delegate) this.m_action;
        return (object) action == null ? "{null}" : action.Method.ToString();
      }
    }

    [SecuritySafeCritical]
    internal void PossiblyCaptureContext(ref StackCrawlMark stackMark) => this.CapturedContext = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);

    internal TaskCreationOptions Options => Task.OptionsMethod(this.m_stateFlags);

    internal static TaskCreationOptions OptionsMethod(int flags) => (TaskCreationOptions) (flags & (int) ushort.MaxValue);

    internal bool AtomicStateUpdate(int newBits, int illegalBits)
    {
      System.Threading.SpinWait spinWait = new System.Threading.SpinWait();
      while (true)
      {
        int stateFlags = this.m_stateFlags;
        if ((stateFlags & illegalBits) == 0)
        {
          if (Interlocked.CompareExchange(ref this.m_stateFlags, stateFlags | newBits, stateFlags) != stateFlags)
            spinWait.SpinOnce();
          else
            goto label_4;
        }
        else
          break;
      }
      return false;
label_4:
      return true;
    }

    internal bool AtomicStateUpdate(int newBits, int illegalBits, ref int oldFlags)
    {
      System.Threading.SpinWait spinWait = new System.Threading.SpinWait();
      while (true)
      {
        oldFlags = this.m_stateFlags;
        if ((oldFlags & illegalBits) == 0)
        {
          if (Interlocked.CompareExchange(ref this.m_stateFlags, oldFlags | newBits, oldFlags) != oldFlags)
            spinWait.SpinOnce();
          else
            goto label_4;
        }
        else
          break;
      }
      return false;
label_4:
      return true;
    }

    internal void SetNotificationForWaitCompletion(bool enabled)
    {
      if (enabled)
      {
        this.AtomicStateUpdate(268435456, 90177536);
      }
      else
      {
        System.Threading.SpinWait spinWait = new System.Threading.SpinWait();
        while (true)
        {
          int stateFlags = this.m_stateFlags;
          if (Interlocked.CompareExchange(ref this.m_stateFlags, stateFlags & -268435457, stateFlags) != stateFlags)
            spinWait.SpinOnce();
          else
            break;
        }
      }
    }

    internal bool NotifyDebuggerOfWaitCompletionIfNecessary()
    {
      if (!this.IsWaitNotificationEnabled || !this.ShouldNotifyDebuggerOfWaitCompletion)
        return false;
      this.NotifyDebuggerOfWaitCompletion();
      return true;
    }

    internal static bool AnyTaskRequiresNotifyDebuggerOfWaitCompletion(Task[] tasks)
    {
      foreach (Task task in tasks)
      {
        if (task != null && task.IsWaitNotificationEnabled && task.ShouldNotifyDebuggerOfWaitCompletion)
          return true;
      }
      return false;
    }

    internal bool IsWaitNotificationEnabledOrNotRanToCompletion
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (this.m_stateFlags & 285212672) != 16777216;
    }

    internal virtual bool ShouldNotifyDebuggerOfWaitCompletion => this.IsWaitNotificationEnabled;

    internal bool IsWaitNotificationEnabled => (this.m_stateFlags & 268435456) != 0;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void NotifyDebuggerOfWaitCompletion() => this.SetNotificationForWaitCompletion(false);

    internal bool MarkStarted() => this.AtomicStateUpdate(65536, 4259840);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool FireTaskScheduledIfNeeded(TaskScheduler ts)
    {
      TplEtwProvider log = TplEtwProvider.Log;
      if (!log.IsEnabled() || (this.m_stateFlags & 1073741824) != 0)
        return false;
      this.m_stateFlags |= 1073741824;
      Task internalCurrent = Task.InternalCurrent;
      Task parent = this.m_parent;
      log.TaskScheduled(ts.Id, internalCurrent == null ? 0 : internalCurrent.Id, this.Id, parent == null ? 0 : parent.Id, (int) this.Options, Thread.GetDomainID());
      return true;
    }

    internal void AddNewChild()
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized(true);
      if (contingentProperties.m_completionCountdown == 1 && !this.IsSelfReplicatingRoot)
        ++contingentProperties.m_completionCountdown;
      else
        Interlocked.Increment(ref contingentProperties.m_completionCountdown);
    }

    internal void DisregardChild() => Interlocked.Decrement(ref this.EnsureContingentPropertiesInitialized(true).m_completionCountdown);

    [__DynamicallyInvokable]
    public void Start() => this.Start(TaskScheduler.Current);

    [__DynamicallyInvokable]
    public void Start(TaskScheduler scheduler)
    {
      int stateFlags = this.m_stateFlags;
      if (Task.IsCompletedMethod(stateFlags))
        throw new InvalidOperationException(Environment.GetResourceString("Task_Start_TaskCompleted"));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      TaskCreationOptions taskCreationOptions = Task.OptionsMethod(stateFlags);
      if ((taskCreationOptions & (TaskCreationOptions) 1024) != TaskCreationOptions.None)
        throw new InvalidOperationException(Environment.GetResourceString("Task_Start_Promise"));
      if ((taskCreationOptions & (TaskCreationOptions) 512) != TaskCreationOptions.None)
        throw new InvalidOperationException(Environment.GetResourceString("Task_Start_ContinuationTask"));
      if (Interlocked.CompareExchange<TaskScheduler>(ref this.m_taskScheduler, scheduler, (TaskScheduler) null) != null)
        throw new InvalidOperationException(Environment.GetResourceString("Task_Start_AlreadyStarted"));
      this.ScheduleAndStart(true);
    }

    [__DynamicallyInvokable]
    public void RunSynchronously() => this.InternalRunSynchronously(TaskScheduler.Current, true);

    [__DynamicallyInvokable]
    public void RunSynchronously(TaskScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      this.InternalRunSynchronously(scheduler, true);
    }

    [SecuritySafeCritical]
    internal void InternalRunSynchronously(TaskScheduler scheduler, bool waitForCompletion)
    {
      int stateFlags = this.m_stateFlags;
      TaskCreationOptions taskCreationOptions = Task.OptionsMethod(stateFlags);
      if ((taskCreationOptions & (TaskCreationOptions) 512) != TaskCreationOptions.None)
        throw new InvalidOperationException(Environment.GetResourceString("Task_RunSynchronously_Continuation"));
      if ((taskCreationOptions & (TaskCreationOptions) 1024) != TaskCreationOptions.None)
        throw new InvalidOperationException(Environment.GetResourceString("Task_RunSynchronously_Promise"));
      if (Task.IsCompletedMethod(stateFlags))
        throw new InvalidOperationException(Environment.GetResourceString("Task_RunSynchronously_TaskCompleted"));
      if (Interlocked.CompareExchange<TaskScheduler>(ref this.m_taskScheduler, scheduler, (TaskScheduler) null) != null)
        throw new InvalidOperationException(Environment.GetResourceString("Task_RunSynchronously_AlreadyStarted"));
      if (!this.MarkStarted())
        throw new InvalidOperationException(Environment.GetResourceString("Task_RunSynchronously_TaskCompleted"));
      bool flag = false;
      try
      {
        if (!scheduler.TryRunInline(this, false))
        {
          scheduler.InternalQueueTask(this);
          flag = true;
        }
        if (!waitForCompletion || this.IsCompleted)
          return;
        this.SpinThenBlockingWait(-1, new CancellationToken());
      }
      catch (System.Exception ex)
      {
        if (!flag && !(ex is ThreadAbortException))
        {
          TaskSchedulerException exceptionObject = new TaskSchedulerException(ex);
          this.AddException((object) exceptionObject);
          this.Finish(false);
          this.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(false);
          throw exceptionObject;
        }
        throw;
      }
    }

    internal static Task InternalStartNew(
      Task creatingTask,
      Delegate action,
      object state,
      CancellationToken cancellationToken,
      TaskScheduler scheduler,
      TaskCreationOptions options,
      InternalTaskOptions internalOptions,
      ref StackCrawlMark stackMark)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      Task task = new Task(action, state, creatingTask, cancellationToken, options, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
      task.PossiblyCaptureContext(ref stackMark);
      task.ScheduleAndStart(false);
      return task;
    }

    internal static int NewId()
    {
      int TaskID;
      do
      {
        TaskID = Interlocked.Increment(ref Task.s_taskIdCounter);
      }
      while (TaskID == 0);
      TplEtwProvider.Log.NewID(TaskID);
      return TaskID;
    }

    [__DynamicallyInvokable]
    public int Id
    {
      [__DynamicallyInvokable] get
      {
        if (this.m_taskId == 0)
          Interlocked.CompareExchange(ref this.m_taskId, Task.NewId(), 0);
        return this.m_taskId;
      }
    }

    [__DynamicallyInvokable]
    public static int? CurrentId
    {
      [__DynamicallyInvokable] get => Task.InternalCurrent?.Id;
    }

    internal static Task InternalCurrent => Task.t_currentTask;

    internal static Task InternalCurrentIfAttached(TaskCreationOptions creationOptions) => (creationOptions & TaskCreationOptions.AttachedToParent) == TaskCreationOptions.None ? (Task) null : Task.InternalCurrent;

    internal static StackGuard CurrentStackGuard
    {
      get
      {
        StackGuard currentStackGuard = Task.t_stackGuard;
        if (currentStackGuard == null)
          Task.t_stackGuard = currentStackGuard = new StackGuard();
        return currentStackGuard;
      }
    }

    [__DynamicallyInvokable]
    public AggregateException Exception
    {
      [__DynamicallyInvokable] get
      {
        AggregateException exception = (AggregateException) null;
        if (this.IsFaulted)
          exception = this.GetExceptions(false);
        return exception;
      }
    }

    [__DynamicallyInvokable]
    public TaskStatus Status
    {
      [__DynamicallyInvokable] get
      {
        int stateFlags = this.m_stateFlags;
        return (stateFlags & 2097152) == 0 ? ((stateFlags & 4194304) == 0 ? ((stateFlags & 16777216) == 0 ? ((stateFlags & 8388608) == 0 ? ((stateFlags & 131072) == 0 ? ((stateFlags & 65536) == 0 ? ((stateFlags & 33554432) == 0 ? TaskStatus.Created : TaskStatus.WaitingForActivation) : TaskStatus.WaitingToRun) : TaskStatus.Running) : TaskStatus.WaitingForChildrenToComplete) : TaskStatus.RanToCompletion) : TaskStatus.Canceled) : TaskStatus.Faulted;
      }
    }

    [__DynamicallyInvokable]
    public bool IsCanceled
    {
      [__DynamicallyInvokable] get => (this.m_stateFlags & 6291456) == 4194304;
    }

    internal bool IsCancellationRequested
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        if (contingentProperties == null)
          return false;
        return contingentProperties.m_internalCancellationRequested == 1 || contingentProperties.m_cancellationToken.IsCancellationRequested;
      }
    }

    internal Task.ContingentProperties EnsureContingentPropertiesInitialized(bool needsProtection) => this.m_contingentProperties ?? this.EnsureContingentPropertiesInitializedCore(needsProtection);

    private Task.ContingentProperties EnsureContingentPropertiesInitializedCore(bool needsProtection) => needsProtection ? LazyInitializer.EnsureInitialized<Task.ContingentProperties>(ref this.m_contingentProperties, Task.s_createContingentProperties) : (this.m_contingentProperties = new Task.ContingentProperties());

    internal CancellationToken CancellationToken
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        return contingentProperties != null ? contingentProperties.m_cancellationToken : new CancellationToken();
      }
    }

    internal bool IsCancellationAcknowledged => (this.m_stateFlags & 1048576) != 0;

    [__DynamicallyInvokable]
    public bool IsCompleted
    {
      [__DynamicallyInvokable] get => Task.IsCompletedMethod(this.m_stateFlags);
    }

    private static bool IsCompletedMethod(int flags) => (flags & 23068672) != 0;

    internal bool IsRanToCompletion => (this.m_stateFlags & 23068672) == 16777216;

    [__DynamicallyInvokable]
    public TaskCreationOptions CreationOptions
    {
      [__DynamicallyInvokable] get => this.Options & (TaskCreationOptions) -65281;
    }

    [__DynamicallyInvokable]
    WaitHandle IAsyncResult.AsyncWaitHandle
    {
      [__DynamicallyInvokable] get
      {
        if ((this.m_stateFlags & 262144) != 0)
          throw new ObjectDisposedException((string) null, Environment.GetResourceString("Task_ThrowIfDisposed"));
        return this.CompletedEvent.WaitHandle;
      }
    }

    [__DynamicallyInvokable]
    public object AsyncState
    {
      [__DynamicallyInvokable] get => this.m_stateObject;
    }

    [__DynamicallyInvokable]
    bool IAsyncResult.CompletedSynchronously
    {
      [__DynamicallyInvokable] get => false;
    }

    internal TaskScheduler ExecutingTaskScheduler => this.m_taskScheduler;

    [__DynamicallyInvokable]
    public static TaskFactory Factory
    {
      [__DynamicallyInvokable] get => Task.s_factory;
    }

    [__DynamicallyInvokable]
    public static Task CompletedTask
    {
      [__DynamicallyInvokable] get
      {
        Task completedTask = Task.s_completedTask;
        if (completedTask == null)
        {
          CancellationToken ct = new CancellationToken();
          Task.s_completedTask = completedTask = new Task(false, (TaskCreationOptions) 16384, ct);
        }
        return completedTask;
      }
    }

    internal ManualResetEventSlim CompletedEvent
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized(true);
        if (contingentProperties.m_completionEvent == null)
        {
          bool isCompleted = this.IsCompleted;
          ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(isCompleted);
          if (Interlocked.CompareExchange<ManualResetEventSlim>(ref contingentProperties.m_completionEvent, manualResetEventSlim, (ManualResetEventSlim) null) != null)
            manualResetEventSlim.Dispose();
          else if (!isCompleted && this.IsCompleted)
            manualResetEventSlim.Set();
        }
        return contingentProperties.m_completionEvent;
      }
    }

    internal bool IsSelfReplicatingRoot => (this.Options & (TaskCreationOptions) 2304) == (TaskCreationOptions) 2048;

    internal bool IsChildReplica => (this.Options & (TaskCreationOptions) 256) != 0;

    internal int ActiveChildCount
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        return contingentProperties == null ? 0 : contingentProperties.m_completionCountdown - 1;
      }
    }

    internal bool ExceptionRecorded
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        return contingentProperties != null && contingentProperties.m_exceptionsHolder != null && contingentProperties.m_exceptionsHolder.ContainsFaultList;
      }
    }

    [__DynamicallyInvokable]
    public bool IsFaulted
    {
      [__DynamicallyInvokable] get => (this.m_stateFlags & 2097152) != 0;
    }

    internal ExecutionContext CapturedContext
    {
      get
      {
        if ((this.m_stateFlags & 536870912) == 536870912)
          return (ExecutionContext) null;
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        return contingentProperties != null && contingentProperties.m_capturedContext != null ? contingentProperties.m_capturedContext : ExecutionContext.PreAllocatedDefault;
      }
      set
      {
        if (value == null)
        {
          this.m_stateFlags |= 536870912;
        }
        else
        {
          if (value.IsPreAllocatedDefault)
            return;
          this.EnsureContingentPropertiesInitialized(false).m_capturedContext = value;
        }
      }
    }

    private static ExecutionContext CopyExecutionContext(ExecutionContext capturedContext)
    {
      if (capturedContext == null)
        return (ExecutionContext) null;
      return capturedContext.IsPreAllocatedDefault ? ExecutionContext.PreAllocatedDefault : capturedContext.CreateCopy();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if ((this.Options & (TaskCreationOptions) 16384) != TaskCreationOptions.None)
          return;
        if (!this.IsCompleted)
          throw new InvalidOperationException(Environment.GetResourceString("Task_Dispose_NotCompleted"));
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        if (contingentProperties != null)
        {
          ManualResetEventSlim completionEvent = contingentProperties.m_completionEvent;
          if (completionEvent != null)
          {
            contingentProperties.m_completionEvent = (ManualResetEventSlim) null;
            if (!completionEvent.IsSet)
              completionEvent.Set();
            completionEvent.Dispose();
          }
        }
      }
      this.m_stateFlags |= 262144;
    }

    [SecuritySafeCritical]
    internal void ScheduleAndStart(bool needsProtection)
    {
      if (needsProtection)
      {
        if (!this.MarkStarted())
          return;
      }
      else
        this.m_stateFlags |= 65536;
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks(this);
      if (AsyncCausalityTracer.LoggingOn && (this.Options & (TaskCreationOptions) 512) == TaskCreationOptions.None)
        AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, this.Id, "Task: " + ((Delegate) this.m_action).Method.Name, 0UL);
      try
      {
        this.m_taskScheduler.InternalQueueTask(this);
      }
      catch (ThreadAbortException ex)
      {
        this.AddException((object) ex);
        this.FinishThreadAbortedTask(true, false);
      }
      catch (System.Exception ex)
      {
        TaskSchedulerException exceptionObject = new TaskSchedulerException(ex);
        this.AddException((object) exceptionObject);
        this.Finish(false);
        if ((this.Options & (TaskCreationOptions) 512) == TaskCreationOptions.None)
          this.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(false);
        throw exceptionObject;
      }
    }

    internal void AddException(object exceptionObject) => this.AddException(exceptionObject, false);

    internal void AddException(object exceptionObject, bool representsCancellation)
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized(true);
      if (contingentProperties.m_exceptionsHolder == null)
      {
        TaskExceptionHolder taskExceptionHolder = new TaskExceptionHolder(this);
        if (Interlocked.CompareExchange<TaskExceptionHolder>(ref contingentProperties.m_exceptionsHolder, taskExceptionHolder, (TaskExceptionHolder) null) != null)
          taskExceptionHolder.MarkAsHandled(false);
      }
      lock (contingentProperties)
        contingentProperties.m_exceptionsHolder.Add(exceptionObject, representsCancellation);
    }

    private AggregateException GetExceptions(bool includeTaskCanceledExceptions)
    {
      System.Exception includeThisException = (System.Exception) null;
      if (includeTaskCanceledExceptions && this.IsCanceled)
        includeThisException = (System.Exception) new TaskCanceledException(this);
      if (this.ExceptionRecorded)
        return this.m_contingentProperties.m_exceptionsHolder.CreateExceptionObject(false, includeThisException);
      if (includeThisException == null)
        return (AggregateException) null;
      return new AggregateException(new System.Exception[1]
      {
        includeThisException
      });
    }

    internal ReadOnlyCollection<ExceptionDispatchInfo> GetExceptionDispatchInfos() => !this.IsFaulted || !this.ExceptionRecorded ? new ReadOnlyCollection<ExceptionDispatchInfo>((IList<ExceptionDispatchInfo>) new ExceptionDispatchInfo[0]) : this.m_contingentProperties.m_exceptionsHolder.GetExceptionDispatchInfos();

    internal ExceptionDispatchInfo GetCancellationExceptionDispatchInfo() => this.m_contingentProperties?.m_exceptionsHolder?.GetCancellationExceptionDispatchInfo();

    internal void ThrowIfExceptional(bool includeTaskCanceledExceptions)
    {
      System.Exception exceptions = (System.Exception) this.GetExceptions(includeTaskCanceledExceptions);
      if (exceptions != null)
      {
        this.UpdateExceptionObservedStatus();
        throw exceptions;
      }
    }

    internal void UpdateExceptionObservedStatus()
    {
      if (this.m_parent == null || (this.Options & TaskCreationOptions.AttachedToParent) == TaskCreationOptions.None || (this.m_parent.CreationOptions & TaskCreationOptions.DenyChildAttach) != TaskCreationOptions.None || Task.InternalCurrent != this.m_parent)
        return;
      this.m_stateFlags |= 524288;
    }

    internal bool IsExceptionObservedByParent => (this.m_stateFlags & 524288) != 0;

    internal bool IsDelegateInvoked => (this.m_stateFlags & 131072) != 0;

    internal void Finish(bool bUserDelegateExecuted)
    {
      if (!bUserDelegateExecuted)
      {
        this.FinishStageTwo();
      }
      else
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        if (contingentProperties == null || contingentProperties.m_completionCountdown == 1 && !this.IsSelfReplicatingRoot || Interlocked.Decrement(ref contingentProperties.m_completionCountdown) == 0)
          this.FinishStageTwo();
        else
          this.AtomicStateUpdate(8388608, 23068672);
        List<Task> exceptionalChildren = contingentProperties?.m_exceptionalChildren;
        if (exceptionalChildren == null)
          return;
        lock (exceptionalChildren)
          exceptionalChildren.RemoveAll(Task.s_IsExceptionObservedByParentPredicate);
      }
    }

    internal void FinishStageTwo()
    {
      this.AddExceptionsFromChildren();
      int num;
      if (this.ExceptionRecorded)
      {
        num = 2097152;
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this.Id);
      }
      else if (this.IsCancellationRequested && this.IsCancellationAcknowledged)
      {
        num = 4194304;
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Canceled);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this.Id);
      }
      else
      {
        num = 16777216;
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Completed);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this.Id);
      }
      Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | num);
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (contingentProperties != null)
      {
        contingentProperties.SetCompleted();
        contingentProperties.DeregisterCancellationCallback();
      }
      this.FinishStageThree();
    }

    internal void FinishStageThree()
    {
      this.m_action = (object) null;
      if (this.m_parent != null && (this.m_parent.CreationOptions & TaskCreationOptions.DenyChildAttach) == TaskCreationOptions.None && (this.m_stateFlags & (int) ushort.MaxValue & 4) != 0)
        this.m_parent.ProcessChildCompletion(this);
      this.FinishContinuations();
    }

    internal void ProcessChildCompletion(Task childTask)
    {
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (childTask.IsFaulted && !childTask.IsExceptionObservedByParent)
      {
        if (contingentProperties.m_exceptionalChildren == null)
          Interlocked.CompareExchange<List<Task>>(ref contingentProperties.m_exceptionalChildren, new List<Task>(), (List<Task>) null);
        List<Task> exceptionalChildren = contingentProperties.m_exceptionalChildren;
        if (exceptionalChildren != null)
        {
          lock (exceptionalChildren)
            exceptionalChildren.Add(childTask);
        }
      }
      if (Interlocked.Decrement(ref contingentProperties.m_completionCountdown) != 0)
        return;
      this.FinishStageTwo();
    }

    internal void AddExceptionsFromChildren()
    {
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      List<Task> exceptionalChildren = contingentProperties?.m_exceptionalChildren;
      if (exceptionalChildren == null)
        return;
      lock (exceptionalChildren)
      {
        foreach (Task task in exceptionalChildren)
        {
          if (task.IsFaulted && !task.IsExceptionObservedByParent)
            this.AddException((object) task.m_contingentProperties.m_exceptionsHolder.CreateExceptionObject(false, (System.Exception) null));
        }
      }
      contingentProperties.m_exceptionalChildren = (List<Task>) null;
    }

    internal void FinishThreadAbortedTask(bool bTAEAddedToExceptionHolder, bool delegateRan)
    {
      if (bTAEAddedToExceptionHolder)
        this.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(false);
      if (!this.AtomicStateUpdate(134217728, 157286400))
        return;
      this.Finish(delegateRan);
    }

    private void Execute()
    {
      if (this.IsSelfReplicatingRoot)
      {
        Task.ExecuteSelfReplicating(this);
      }
      else
      {
        try
        {
          this.InnerInvoke();
        }
        catch (ThreadAbortException ex)
        {
          if (this.IsChildReplica)
            return;
          this.HandleException((System.Exception) ex);
          this.FinishThreadAbortedTask(true, true);
        }
        catch (System.Exception ex)
        {
          this.HandleException(ex);
        }
      }
    }

    internal virtual bool ShouldReplicate() => true;

    internal virtual Task CreateReplicaTask(
      Action<object> taskReplicaDelegate,
      object stateObject,
      Task parentTask,
      TaskScheduler taskScheduler,
      TaskCreationOptions creationOptionsForReplica,
      InternalTaskOptions internalOptionsForReplica)
    {
      return new Task((Delegate) taskReplicaDelegate, stateObject, parentTask, new CancellationToken(), creationOptionsForReplica, internalOptionsForReplica, parentTask.ExecutingTaskScheduler);
    }

    internal virtual object SavedStateForNextReplica
    {
      get => (object) null;
      set
      {
      }
    }

    internal virtual object SavedStateFromPreviousReplica
    {
      get => (object) null;
      set
      {
      }
    }

    internal virtual Task HandedOverChildReplica
    {
      get => (Task) null;
      set
      {
      }
    }

    private static void ExecuteSelfReplicating(Task root)
    {
      TaskCreationOptions creationOptionsForReplicas = root.CreationOptions | TaskCreationOptions.AttachedToParent;
      InternalTaskOptions internalOptionsForReplicas = InternalTaskOptions.ChildReplica | InternalTaskOptions.SelfReplicating | InternalTaskOptions.QueuedByRuntime;
      bool replicasAreQuitting = false;
      Action<object> taskReplicaDelegate = (Action<object>) null;
      taskReplicaDelegate = (Action<object>) (_param1 =>
      {
        Task internalCurrent = Task.InternalCurrent;
        Task task = internalCurrent.HandedOverChildReplica;
        if (task == null)
        {
          if (!root.ShouldReplicate() || Volatile.Read(ref replicasAreQuitting))
            return;
          ExecutionContext capturedContext = root.CapturedContext;
          task = root.CreateReplicaTask(taskReplicaDelegate, root.m_stateObject, root, root.ExecutingTaskScheduler, creationOptionsForReplicas, internalOptionsForReplicas);
          task.CapturedContext = Task.CopyExecutionContext(capturedContext);
          task.ScheduleAndStart(false);
        }
        try
        {
          root.InnerInvokeWithArg(internalCurrent);
        }
        catch (System.Exception ex)
        {
          root.HandleException(ex);
          if (ex is ThreadAbortException)
            internalCurrent.FinishThreadAbortedTask(false, true);
        }
        object stateForNextReplica = internalCurrent.SavedStateForNextReplica;
        if (stateForNextReplica != null)
        {
          Task replicaTask = root.CreateReplicaTask(taskReplicaDelegate, root.m_stateObject, root, root.ExecutingTaskScheduler, creationOptionsForReplicas, internalOptionsForReplicas);
          ExecutionContext capturedContext = root.CapturedContext;
          replicaTask.CapturedContext = Task.CopyExecutionContext(capturedContext);
          replicaTask.HandedOverChildReplica = task;
          replicaTask.SavedStateFromPreviousReplica = stateForNextReplica;
          replicaTask.ScheduleAndStart(false);
        }
        else
        {
          replicasAreQuitting = true;
          try
          {
            task.InternalCancel(true);
          }
          catch (System.Exception ex)
          {
            root.HandleException(ex);
          }
        }
      });
      taskReplicaDelegate((object) null);
    }

    [SecurityCritical]
    void IThreadPoolWorkItem.ExecuteWorkItem() => this.ExecuteEntry(false);

    [SecurityCritical]
    void IThreadPoolWorkItem.MarkAborted(ThreadAbortException tae)
    {
      if (this.IsCompleted)
        return;
      this.HandleException((System.Exception) tae);
      this.FinishThreadAbortedTask(true, false);
    }

    [SecuritySafeCritical]
    internal bool ExecuteEntry(bool bPreventDoubleExecution)
    {
      if (bPreventDoubleExecution || (this.Options & (TaskCreationOptions) 2048) != TaskCreationOptions.None)
      {
        int oldFlags = 0;
        if (!this.AtomicStateUpdate(131072, 23199744, ref oldFlags) && (oldFlags & 4194304) == 0)
          return false;
      }
      else
        this.m_stateFlags |= 131072;
      if (!this.IsCancellationRequested && !this.IsCanceled)
        this.ExecuteWithThreadLocal(ref Task.t_currentTask);
      else if (!this.IsCanceled && (Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | 4194304) & 4194304) == 0)
        this.CancellationCleanupLogic();
      return true;
    }

    [SecurityCritical]
    private void ExecuteWithThreadLocal(ref Task currentTaskSlot)
    {
      Task task = currentTaskSlot;
      TplEtwProvider log = TplEtwProvider.Log;
      Guid oldActivityThatWillContinue = new Guid();
      bool flag = log.IsEnabled();
      if (flag)
      {
        if (log.TasksSetActivityIds)
          EventSource.SetCurrentThreadActivityId(TplEtwProvider.CreateGuidForTaskID(this.Id), out oldActivityThatWillContinue);
        if (task != null)
          log.TaskStarted(task.m_taskScheduler.Id, task.Id, this.Id);
        else
          log.TaskStarted(TaskScheduler.Current.Id, 0, this.Id);
      }
      if (AsyncCausalityTracer.LoggingOn)
        AsyncCausalityTracer.TraceSynchronousWorkStart(CausalityTraceLevel.Required, this.Id, CausalitySynchronousWork.Execution);
      try
      {
        currentTaskSlot = this;
        ExecutionContext capturedContext = this.CapturedContext;
        if (capturedContext == null)
        {
          this.Execute();
        }
        else
        {
          if (this.IsSelfReplicatingRoot || this.IsChildReplica)
            this.CapturedContext = Task.CopyExecutionContext(capturedContext);
          ContextCallback callback = Task.s_ecCallback;
          if (callback == null)
            Task.s_ecCallback = callback = new ContextCallback(Task.ExecutionContextCallback);
          ExecutionContext.Run(capturedContext, callback, (object) this, true);
        }
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceSynchronousWorkCompletion(CausalityTraceLevel.Required, CausalitySynchronousWork.Execution);
        this.Finish(true);
      }
      finally
      {
        currentTaskSlot = task;
        if (flag)
        {
          if (task != null)
            log.TaskCompleted(task.m_taskScheduler.Id, task.Id, this.Id, this.IsFaulted);
          else
            log.TaskCompleted(TaskScheduler.Current.Id, 0, this.Id, this.IsFaulted);
          if (log.TasksSetActivityIds)
            EventSource.SetCurrentThreadActivityId(oldActivityThatWillContinue);
        }
      }
    }

    [SecurityCritical]
    private static void ExecutionContextCallback(object obj) => (obj as Task).Execute();

    internal virtual void InnerInvoke()
    {
      if (this.m_action is Action action1)
      {
        action1();
      }
      else
      {
        if (!(this.m_action is Action<object> action))
          return;
        action(this.m_stateObject);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal void InnerInvokeWithArg(Task childTask) => this.InnerInvoke();

    private void HandleException(System.Exception unhandledException)
    {
      if (unhandledException is OperationCanceledException exceptionObject && this.IsCancellationRequested && this.m_contingentProperties.m_cancellationToken == exceptionObject.CancellationToken)
      {
        this.SetCancellationAcknowledged();
        this.AddException((object) exceptionObject, true);
      }
      else
        this.AddException((object) unhandledException);
    }

    [__DynamicallyInvokable]
    public TaskAwaiter GetAwaiter() => new TaskAwaiter(this);

    [__DynamicallyInvokable]
    public ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) => new ConfiguredTaskAwaitable(this, continueOnCapturedContext);

    [SecurityCritical]
    internal void SetContinuationForAwait(
      Action continuationAction,
      bool continueOnCapturedContext,
      bool flowExecutionContext,
      ref StackCrawlMark stackMark)
    {
      TaskContinuation tc = (TaskContinuation) null;
      if (continueOnCapturedContext)
      {
        SynchronizationContext currentNoFlow = SynchronizationContext.CurrentNoFlow;
        if (currentNoFlow != null && currentNoFlow.GetType() != typeof (SynchronizationContext))
        {
          tc = (TaskContinuation) new SynchronizationContextAwaitTaskContinuation(currentNoFlow, continuationAction, flowExecutionContext, ref stackMark);
        }
        else
        {
          TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
          if (internalCurrent != null && internalCurrent != TaskScheduler.Default)
            tc = (TaskContinuation) new TaskSchedulerAwaitTaskContinuation(internalCurrent, continuationAction, flowExecutionContext, ref stackMark);
        }
      }
      if (tc == null & flowExecutionContext)
        tc = (TaskContinuation) new AwaitTaskContinuation(continuationAction, true, ref stackMark);
      if (tc != null)
      {
        if (this.AddTaskContinuation((object) tc, false))
          return;
        tc.Run(this, false);
      }
      else
      {
        if (this.AddTaskContinuation((object) continuationAction, false))
          return;
        AwaitTaskContinuation.UnsafeScheduleAction(continuationAction, this);
      }
    }

    [__DynamicallyInvokable]
    public static YieldAwaitable Yield() => new YieldAwaitable();

    [__DynamicallyInvokable]
    public void Wait() => this.Wait(-1, new CancellationToken());

    [__DynamicallyInvokable]
    public bool Wait(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? this.Wait((int) totalMilliseconds, new CancellationToken()) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    [__DynamicallyInvokable]
    public void Wait(CancellationToken cancellationToken) => this.Wait(-1, cancellationToken);

    [__DynamicallyInvokable]
    public bool Wait(int millisecondsTimeout) => this.Wait(millisecondsTimeout, new CancellationToken());

    [__DynamicallyInvokable]
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
      if (!this.IsWaitNotificationEnabledOrNotRanToCompletion)
        return true;
      if (!this.InternalWait(millisecondsTimeout, cancellationToken))
        return false;
      if (this.IsWaitNotificationEnabledOrNotRanToCompletion)
      {
        this.NotifyDebuggerOfWaitCompletionIfNecessary();
        if (this.IsCanceled)
          cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfExceptional(true);
      }
      return true;
    }

    private bool WrappedTryRunInline()
    {
      if (this.m_taskScheduler == null)
        return false;
      try
      {
        return this.m_taskScheduler.TryRunInline(this, true);
      }
      catch (System.Exception ex)
      {
        if (!(ex is ThreadAbortException))
          throw new TaskSchedulerException(ex);
        throw;
      }
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      TplEtwProvider log = TplEtwProvider.Log;
      bool flag1 = log.IsEnabled();
      if (flag1)
      {
        Task internalCurrent = Task.InternalCurrent;
        log.TaskWaitBegin(internalCurrent != null ? internalCurrent.m_taskScheduler.Id : TaskScheduler.Default.Id, internalCurrent != null ? internalCurrent.Id : 0, this.Id, TplEtwProvider.TaskWaitBehavior.Synchronous, 0, Thread.GetDomainID());
      }
      bool flag2 = this.IsCompleted;
      if (!flag2)
      {
        Debugger.NotifyOfCrossThreadDependency();
        flag2 = millisecondsTimeout == -1 && !cancellationToken.CanBeCanceled && this.WrappedTryRunInline() && this.IsCompleted || this.SpinThenBlockingWait(millisecondsTimeout, cancellationToken);
      }
      if (flag1)
      {
        Task internalCurrent = Task.InternalCurrent;
        if (internalCurrent != null)
          log.TaskWaitEnd(internalCurrent.m_taskScheduler.Id, internalCurrent.Id, this.Id);
        else
          log.TaskWaitEnd(TaskScheduler.Default.Id, 0, this.Id);
        log.TaskWaitContinuationComplete(this.Id);
      }
      return flag2;
    }

    private bool SpinThenBlockingWait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      bool flag1 = millisecondsTimeout == -1;
      uint num1 = flag1 ? 0U : (uint) Environment.TickCount;
      bool flag2 = this.SpinWait(millisecondsTimeout);
      if (!flag2)
      {
        Task.SetOnInvokeMres setOnInvokeMres = new Task.SetOnInvokeMres();
        try
        {
          this.AddCompletionAction((ITaskCompletionAction) setOnInvokeMres, true);
          if (flag1)
          {
            flag2 = setOnInvokeMres.Wait(-1, cancellationToken);
          }
          else
          {
            uint num2 = (uint) Environment.TickCount - num1;
            if ((long) num2 < (long) millisecondsTimeout)
              flag2 = setOnInvokeMres.Wait((int) ((long) millisecondsTimeout - (long) num2), cancellationToken);
          }
        }
        finally
        {
          if (!this.IsCompleted)
            this.RemoveContinuation((object) setOnInvokeMres);
        }
      }
      return flag2;
    }

    private bool SpinWait(int millisecondsTimeout)
    {
      if (this.IsCompleted)
        return true;
      if (millisecondsTimeout == 0)
        return false;
      int num = PlatformHelper.IsSingleProcessor ? 1 : 10;
      for (int index = 0; index < num; ++index)
      {
        if (this.IsCompleted)
          return true;
        if (index == num / 2)
          Thread.Yield();
        else
          Thread.SpinWait(4 << index);
      }
      return this.IsCompleted;
    }

    [SecuritySafeCritical]
    internal bool InternalCancel(bool bCancelNonExecutingOnly)
    {
      bool flag1 = false;
      bool flag2 = false;
      TaskSchedulerException schedulerException = (TaskSchedulerException) null;
      if ((this.m_stateFlags & 65536) != 0)
      {
        TaskScheduler taskScheduler = this.m_taskScheduler;
        try
        {
          flag1 = taskScheduler != null && taskScheduler.TryDequeue(this);
        }
        catch (System.Exception ex)
        {
          if (!(ex is ThreadAbortException))
            schedulerException = new TaskSchedulerException(ex);
        }
        bool flag3 = taskScheduler != null && taskScheduler.RequiresAtomicStartTransition || (this.Options & (TaskCreationOptions) 2048) != 0;
        if (!flag1 & bCancelNonExecutingOnly & flag3)
          flag2 = this.AtomicStateUpdate(4194304, 4325376);
      }
      if (!bCancelNonExecutingOnly | flag1 | flag2)
      {
        this.RecordInternalCancellationRequest();
        if (flag1)
          flag2 = this.AtomicStateUpdate(4194304, 4325376);
        else if (!flag2 && (this.m_stateFlags & 65536) == 0)
          flag2 = this.AtomicStateUpdate(4194304, 23265280);
        if (flag2)
          this.CancellationCleanupLogic();
      }
      if (schedulerException != null)
        throw schedulerException;
      return flag2;
    }

    internal void RecordInternalCancellationRequest() => this.EnsureContingentPropertiesInitialized(true).m_internalCancellationRequested = 1;

    internal void RecordInternalCancellationRequest(CancellationToken tokenToRecord)
    {
      this.RecordInternalCancellationRequest();
      if (!(tokenToRecord != new CancellationToken()))
        return;
      this.m_contingentProperties.m_cancellationToken = tokenToRecord;
    }

    internal void RecordInternalCancellationRequest(
      CancellationToken tokenToRecord,
      object cancellationException)
    {
      this.RecordInternalCancellationRequest(tokenToRecord);
      if (cancellationException == null)
        return;
      this.AddException(cancellationException, true);
    }

    internal void CancellationCleanupLogic()
    {
      Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | 4194304);
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (contingentProperties != null)
      {
        contingentProperties.SetCompleted();
        contingentProperties.DeregisterCancellationCallback();
      }
      if (AsyncCausalityTracer.LoggingOn)
        AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Canceled);
      if (Task.s_asyncDebuggingEnabled)
        Task.RemoveFromActiveTasks(this.Id);
      this.FinishStageThree();
    }

    private void SetCancellationAcknowledged() => this.m_stateFlags |= 1048576;

    [SecuritySafeCritical]
    internal void FinishContinuations()
    {
      object Object1 = Interlocked.Exchange(ref this.m_continuationObject, Task.s_taskCompletionSentinel);
      TplEtwProvider.Log.RunningContinuation(this.Id, Object1);
      if (Object1 == null)
        return;
      if (AsyncCausalityTracer.LoggingOn)
        AsyncCausalityTracer.TraceSynchronousWorkStart(CausalityTraceLevel.Required, this.Id, CausalitySynchronousWork.CompletionNotification);
      bool flag = (this.m_stateFlags & 134217728) == 0 && Thread.CurrentThread.ThreadState != ThreadState.AbortRequested && (this.m_stateFlags & 64) == 0;
      switch (Object1)
      {
        case Action action2:
          AwaitTaskContinuation.RunOrScheduleAction(action2, flag, ref Task.t_currentTask);
          this.LogFinishCompletionNotification();
          break;
        case ITaskCompletionAction action3:
          if (flag)
            action3.Invoke(this);
          else
            ThreadPool.UnsafeQueueCustomWorkItem((IThreadPoolWorkItem) new CompletionActionInvoker(action3, this), false);
          this.LogFinishCompletionNotification();
          break;
        case TaskContinuation taskContinuation1:
          taskContinuation1.Run(this, flag);
          this.LogFinishCompletionNotification();
          break;
        case List<object> objectList:
          lock (objectList)
            ;
          int count = objectList.Count;
          for (int index = 0; index < count; ++index)
          {
            if (objectList[index] is StandardTaskContinuation taskContinuation && (taskContinuation.m_options & TaskContinuationOptions.ExecuteSynchronously) == TaskContinuationOptions.None)
            {
              TplEtwProvider.Log.RunningContinuationList(this.Id, index, (object) taskContinuation);
              objectList[index] = (object) null;
              taskContinuation.Run(this, flag);
            }
          }
          for (int index = 0; index < count; ++index)
          {
            object Object2 = objectList[index];
            if (Object2 != null)
            {
              objectList[index] = (object) null;
              TplEtwProvider.Log.RunningContinuationList(this.Id, index, Object2);
              if (Object2 is Action action1)
                AwaitTaskContinuation.RunOrScheduleAction(action1, flag, ref Task.t_currentTask);
              else if (Object2 is TaskContinuation taskContinuation)
              {
                taskContinuation.Run(this, flag);
              }
              else
              {
                ITaskCompletionAction action = (ITaskCompletionAction) Object2;
                if (flag)
                  action.Invoke(this);
                else
                  ThreadPool.UnsafeQueueCustomWorkItem((IThreadPoolWorkItem) new CompletionActionInvoker(action, this), false);
              }
            }
          }
          this.LogFinishCompletionNotification();
          break;
        default:
          this.LogFinishCompletionNotification();
          break;
      }
    }

    private void LogFinishCompletionNotification()
    {
      if (!AsyncCausalityTracer.LoggingOn)
        return;
      AsyncCausalityTracer.TraceSynchronousWorkCompletion(CausalityTraceLevel.Required, CausalitySynchronousWork.CompletionNotification);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(Action<Task> continuationAction)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, scheduler, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), continuationOptions, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions, ref stackMark);
    }

    private Task ContinueWith(
      Action<Task> continuationAction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      ref StackCrawlMark stackMark)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromTask(this, (Delegate) continuationAction, (object) null, creationOptions, internalOptions, ref stackMark);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(Action<Task, object> continuationAction, object state)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      TaskContinuationOptions continuationOptions)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith(continuationAction, state, scheduler, cancellationToken, continuationOptions, ref stackMark);
    }

    private Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      ref StackCrawlMark stackMark)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromTask(this, (Delegate) continuationAction, state, creationOptions, internalOptions, ref stackMark);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, scheduler, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), continuationOptions, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, scheduler, cancellationToken, continuationOptions, ref stackMark);
    }

    private Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      ref StackCrawlMark stackMark)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TResult> continuationTask = (Task<TResult>) new ContinuationResultTaskFromTask<TResult>(this, (Delegate) continuationFunction, (object) null, creationOptions, internalOptions, ref stackMark);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      TaskContinuationOptions continuationOptions)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return this.ContinueWith<TResult>(continuationFunction, state, scheduler, cancellationToken, continuationOptions, ref stackMark);
    }

    private Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      ref StackCrawlMark stackMark)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TResult> continuationTask = (Task<TResult>) new ContinuationResultTaskFromTask<TResult>(this, (Delegate) continuationFunction, state, creationOptions, internalOptions, ref stackMark);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    internal static void CreationOptionsFromContinuationOptions(
      TaskContinuationOptions continuationOptions,
      out TaskCreationOptions creationOptions,
      out InternalTaskOptions internalOptions)
    {
      TaskContinuationOptions continuationOptions1 = TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion;
      TaskContinuationOptions continuationOptions2 = TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.RunContinuationsAsynchronously;
      TaskContinuationOptions continuationOptions3 = TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously;
      if ((continuationOptions & continuationOptions3) == continuationOptions3)
        throw new ArgumentOutOfRangeException(nameof (continuationOptions), Environment.GetResourceString("Task_ContinueWith_ESandLR"));
      if ((continuationOptions & ~(continuationOptions2 | continuationOptions1 | TaskContinuationOptions.LazyCancellation | TaskContinuationOptions.ExecuteSynchronously)) != TaskContinuationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (continuationOptions));
      if ((continuationOptions & continuationOptions1) == continuationOptions1)
        throw new ArgumentOutOfRangeException(nameof (continuationOptions), Environment.GetResourceString("Task_ContinueWith_NotOnAnything"));
      creationOptions = (TaskCreationOptions) (continuationOptions & continuationOptions2);
      internalOptions = InternalTaskOptions.ContinuationTask;
      if ((continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
        return;
      internalOptions |= InternalTaskOptions.LazyCancellation;
    }

    internal void ContinueWithCore(
      Task continuationTask,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions options)
    {
      TaskContinuation taskContinuation = (TaskContinuation) new StandardTaskContinuation(continuationTask, options, scheduler);
      if (cancellationToken.CanBeCanceled)
      {
        if (this.IsCompleted || cancellationToken.IsCancellationRequested)
          continuationTask.AssignCancellationToken(cancellationToken, (Task) null, (TaskContinuation) null);
        else
          continuationTask.AssignCancellationToken(cancellationToken, this, taskContinuation);
      }
      if (continuationTask.IsCompleted)
        return;
      if ((this.Options & (TaskCreationOptions) 1024) != TaskCreationOptions.None && !(this is ITaskCompletionAction))
      {
        TplEtwProvider log = TplEtwProvider.Log;
        if (log.IsEnabled())
          log.AwaitTaskContinuationScheduled(TaskScheduler.Current.Id, Task.CurrentId ?? 0, continuationTask.Id);
      }
      if (this.AddTaskContinuation((object) taskContinuation, false))
        return;
      taskContinuation.Run(this, true);
    }

    internal void AddCompletionAction(ITaskCompletionAction action) => this.AddCompletionAction(action, false);

    private void AddCompletionAction(ITaskCompletionAction action, bool addBeforeOthers)
    {
      if (this.AddTaskContinuation((object) action, addBeforeOthers))
        return;
      action.Invoke(this);
    }

    private bool AddTaskContinuationComplex(object tc, bool addBeforeOthers)
    {
      object continuationObject1 = this.m_continuationObject;
      if (continuationObject1 != Task.s_taskCompletionSentinel && !(continuationObject1 is List<object>))
        Interlocked.CompareExchange(ref this.m_continuationObject, (object) new List<object>()
        {
          continuationObject1
        }, continuationObject1);
      if (this.m_continuationObject is List<object> continuationObject2)
      {
        lock (continuationObject2)
        {
          if (this.m_continuationObject != Task.s_taskCompletionSentinel)
          {
            if (continuationObject2.Count == continuationObject2.Capacity)
              continuationObject2.RemoveAll(Task.s_IsTaskContinuationNullPredicate);
            if (addBeforeOthers)
              continuationObject2.Insert(0, tc);
            else
              continuationObject2.Add(tc);
            return true;
          }
        }
      }
      return false;
    }

    private bool AddTaskContinuation(object tc, bool addBeforeOthers)
    {
      if (this.IsCompleted)
        return false;
      return this.m_continuationObject == null && Interlocked.CompareExchange(ref this.m_continuationObject, tc, (object) null) == null || this.AddTaskContinuationComplex(tc, addBeforeOthers);
    }

    internal void RemoveContinuation(object continuationObject)
    {
      object continuationObject1 = this.m_continuationObject;
      if (continuationObject1 == Task.s_taskCompletionSentinel)
        return;
      if (!(continuationObject1 is List<object> objectList))
      {
        if (Interlocked.CompareExchange(ref this.m_continuationObject, (object) new List<object>(), continuationObject) == continuationObject)
          return;
        objectList = this.m_continuationObject as List<object>;
      }
      if (objectList == null)
        return;
      lock (objectList)
      {
        if (this.m_continuationObject == Task.s_taskCompletionSentinel)
          return;
        int index = objectList.IndexOf(continuationObject);
        if (index == -1)
          return;
        objectList[index] = (object) null;
      }
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void WaitAll(params Task[] tasks) => Task.WaitAll(tasks, -1);

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(Task[] tasks, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? Task.WaitAll(tasks, (int) totalMilliseconds) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(Task[] tasks, int millisecondsTimeout) => Task.WaitAll(tasks, millisecondsTimeout, new CancellationToken());

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void WaitAll(Task[] tasks, CancellationToken cancellationToken) => Task.WaitAll(tasks, -1, cancellationToken);

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
      cancellationToken.ThrowIfCancellationRequested();
      List<System.Exception> exceptions = (List<System.Exception>) null;
      List<Task> list1 = (List<Task>) null;
      List<Task> list2 = (List<Task>) null;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = true;
      for (int index = tasks.Length - 1; index >= 0; --index)
      {
        Task task = tasks[index];
        bool flag4 = task != null ? task.IsCompleted : throw new ArgumentException(Environment.GetResourceString("Task_WaitMulti_NullTask"), nameof (tasks));
        if (!flag4)
        {
          if (millisecondsTimeout != -1 || cancellationToken.CanBeCanceled)
          {
            Task.AddToList<Task>(task, ref list1, tasks.Length);
          }
          else
          {
            flag4 = task.WrappedTryRunInline() && task.IsCompleted;
            if (!flag4)
              Task.AddToList<Task>(task, ref list1, tasks.Length);
          }
        }
        if (flag4)
        {
          if (task.IsFaulted)
            flag1 = true;
          else if (task.IsCanceled)
            flag2 = true;
          if (task.IsWaitNotificationEnabled)
            Task.AddToList<Task>(task, ref list2, 1);
        }
      }
      if (list1 != null)
      {
        flag3 = Task.WaitAllBlockingCore(list1, millisecondsTimeout, cancellationToken);
        if (flag3)
        {
          foreach (Task task in list1)
          {
            if (task.IsFaulted)
              flag1 = true;
            else if (task.IsCanceled)
              flag2 = true;
            if (task.IsWaitNotificationEnabled)
              Task.AddToList<Task>(task, ref list2, 1);
          }
        }
        GC.KeepAlive((object) tasks);
      }
      if (flag3 && list2 != null)
      {
        foreach (Task task in list2)
        {
          if (task.NotifyDebuggerOfWaitCompletionIfNecessary())
            break;
        }
      }
      if (flag3 && flag1 | flag2)
      {
        if (!flag1)
          cancellationToken.ThrowIfCancellationRequested();
        foreach (Task task in tasks)
          Task.AddExceptionsForCompletedTask(ref exceptions, task);
        throw new AggregateException((IEnumerable<System.Exception>) exceptions);
      }
      return flag3;
    }

    private static void AddToList<T>(T item, ref List<T> list, int initSize)
    {
      if (list == null)
        list = new List<T>(initSize);
      list.Add(item);
    }

    private static bool WaitAllBlockingCore(
      List<Task> tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      bool flag = false;
      Task.SetOnCountdownMres setOnCountdownMres = new Task.SetOnCountdownMres(tasks.Count);
      try
      {
        foreach (Task task in tasks)
          task.AddCompletionAction((ITaskCompletionAction) setOnCountdownMres, true);
        flag = setOnCountdownMres.Wait(millisecondsTimeout, cancellationToken);
        return flag;
      }
      finally
      {
        if (!flag)
        {
          foreach (Task task in tasks)
          {
            if (!task.IsCompleted)
              task.RemoveContinuation((object) setOnCountdownMres);
          }
        }
      }
    }

    internal static void FastWaitAll(Task[] tasks)
    {
      List<System.Exception> exceptions = (List<System.Exception>) null;
      for (int index = tasks.Length - 1; index >= 0; --index)
      {
        if (!tasks[index].IsCompleted)
          tasks[index].WrappedTryRunInline();
      }
      for (int index = tasks.Length - 1; index >= 0; --index)
      {
        Task task = tasks[index];
        task.SpinThenBlockingWait(-1, new CancellationToken());
        Task.AddExceptionsForCompletedTask(ref exceptions, task);
      }
      if (exceptions != null)
        throw new AggregateException((IEnumerable<System.Exception>) exceptions);
    }

    internal static void AddExceptionsForCompletedTask(ref List<System.Exception> exceptions, Task t)
    {
      AggregateException exceptions1 = t.GetExceptions(true);
      if (exceptions1 == null)
        return;
      t.UpdateExceptionObservedStatus();
      if (exceptions == null)
        exceptions = new List<System.Exception>(exceptions1.InnerExceptions.Count);
      exceptions.AddRange((IEnumerable<System.Exception>) exceptions1.InnerExceptions);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(params Task[] tasks) => Task.WaitAny(tasks, -1);

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? Task.WaitAny(tasks, (int) totalMilliseconds) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, CancellationToken cancellationToken) => Task.WaitAny(tasks, -1, cancellationToken);

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, int millisecondsTimeout) => Task.WaitAny(tasks, millisecondsTimeout, new CancellationToken());

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
      cancellationToken.ThrowIfCancellationRequested();
      int num = -1;
      for (int index = 0; index < tasks.Length; ++index)
      {
        Task task = tasks[index];
        if (task == null)
          throw new ArgumentException(Environment.GetResourceString("Task_WaitMulti_NullTask"), nameof (tasks));
        if (num == -1 && task.IsCompleted)
          num = index;
      }
      if (num == -1 && tasks.Length != 0)
      {
        Task<Task> task = TaskFactory.CommonCWAnyLogic((IList<Task>) tasks);
        if (task.Wait(millisecondsTimeout, cancellationToken))
          num = Array.IndexOf<Task>(tasks, task.Result);
      }
      GC.KeepAlive((object) tasks);
      return num;
    }

    [__DynamicallyInvokable]
    public static Task<TResult> FromResult<TResult>(TResult result) => new Task<TResult>(result);

    [__DynamicallyInvokable]
    public static Task FromException(System.Exception exception) => (Task) Task.FromException<VoidTaskResult>(exception);

    [__DynamicallyInvokable]
    public static Task<TResult> FromException<TResult>(System.Exception exception)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      Task<TResult> task = new Task<TResult>();
      task.TrySetException((object) exception);
      return task;
    }

    [FriendAccessAllowed]
    internal static Task FromCancellation(CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested ? new Task(true, TaskCreationOptions.None, cancellationToken) : throw new ArgumentOutOfRangeException(nameof (cancellationToken));

    [__DynamicallyInvokable]
    public static Task FromCanceled(CancellationToken cancellationToken) => Task.FromCancellation(cancellationToken);

    [FriendAccessAllowed]
    internal static Task<TResult> FromCancellation<TResult>(CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested ? new Task<TResult>(true, default (TResult), TaskCreationOptions.None, cancellationToken) : throw new ArgumentOutOfRangeException(nameof (cancellationToken));

    [__DynamicallyInvokable]
    public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken) => Task.FromCancellation<TResult>(cancellationToken);

    internal static Task<TResult> FromCancellation<TResult>(OperationCanceledException exception)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      Task<TResult> task = new Task<TResult>();
      task.TrySetCanceled(exception.CancellationToken, (object) exception);
      return task;
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Task Run(Action action)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Task.InternalStartNew((Task) null, (Delegate) action, (object) null, new CancellationToken(), TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Task Run(Action action, CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Task.InternalStartNew((Task) null, (Delegate) action, (object) null, cancellationToken, TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Task<TResult> Run<TResult>(Func<TResult> function)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Task<TResult>.StartNew((Task) null, function, new CancellationToken(), TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default, ref stackMark);
    }

    [__DynamicallyInvokable]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Task<TResult> Run<TResult>(
      Func<TResult> function,
      CancellationToken cancellationToken)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Task<TResult>.StartNew((Task) null, function, cancellationToken, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default, ref stackMark);
    }

    [__DynamicallyInvokable]
    public static Task Run(Func<Task> function) => Task.Run(function, new CancellationToken());

    [__DynamicallyInvokable]
    public static Task Run(Func<Task> function, CancellationToken cancellationToken)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (AppContextSwitches.ThrowExceptionIfDisposedCancellationTokenSource)
        cancellationToken.ThrowIfSourceDisposed();
      return cancellationToken.IsCancellationRequested ? Task.FromCancellation(cancellationToken) : (Task) new UnwrapPromise<VoidTaskResult>((Task) Task<Task>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default), true);
    }

    [__DynamicallyInvokable]
    public static Task<TResult> Run<TResult>(Func<Task<TResult>> function) => Task.Run<TResult>(function, new CancellationToken());

    [__DynamicallyInvokable]
    public static Task<TResult> Run<TResult>(
      Func<Task<TResult>> function,
      CancellationToken cancellationToken)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (AppContextSwitches.ThrowExceptionIfDisposedCancellationTokenSource)
        cancellationToken.ThrowIfSourceDisposed();
      return cancellationToken.IsCancellationRequested ? Task.FromCancellation<TResult>(cancellationToken) : (Task<TResult>) new UnwrapPromise<TResult>((Task) Task<Task<TResult>>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default), true);
    }

    [__DynamicallyInvokable]
    public static Task Delay(TimeSpan delay) => Task.Delay(delay, new CancellationToken());

    [__DynamicallyInvokable]
    public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) delay.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? Task.Delay((int) totalMilliseconds, cancellationToken) : throw new ArgumentOutOfRangeException(nameof (delay), Environment.GetResourceString("Task_Delay_InvalidDelay"));
    }

    [__DynamicallyInvokable]
    public static Task Delay(int millisecondsDelay) => Task.Delay(millisecondsDelay, new CancellationToken());

    [__DynamicallyInvokable]
    public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
    {
      if (millisecondsDelay < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsDelay), Environment.GetResourceString("Task_Delay_InvalidMillisecondsDelay"));
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCancellation(cancellationToken);
      if (millisecondsDelay == 0)
        return Task.CompletedTask;
      Task.DelayPromise state1 = new Task.DelayPromise(cancellationToken);
      if (cancellationToken.CanBeCanceled)
        state1.Registration = cancellationToken.InternalRegisterWithoutEC((Action<object>) (state => ((Task.DelayPromise) state).Complete()), (object) state1);
      if (millisecondsDelay != -1)
      {
        state1.Timer = new Timer((TimerCallback) (state => ((Task.DelayPromise) state).Complete()), (object) state1, millisecondsDelay, -1);
        state1.Timer.KeepRootedWhileScheduled();
      }
      return (Task) state1;
    }

    [__DynamicallyInvokable]
    public static Task WhenAll(IEnumerable<Task> tasks)
    {
      switch (tasks)
      {
        case Task[] taskArray:
          return Task.WhenAll(taskArray);
        case ICollection<Task> tasks2:
          int num = 0;
          Task[] tasks1 = new Task[tasks2.Count];
          foreach (Task task in tasks)
            tasks1[num++] = task != null ? task : throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
          return Task.InternalWhenAll(tasks1);
        case null:
          throw new ArgumentNullException(nameof (tasks));
        default:
          List<Task> taskList = new List<Task>();
          foreach (Task task in tasks)
          {
            if (task == null)
              throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
            taskList.Add(task);
          }
          return Task.InternalWhenAll(taskList.ToArray());
      }
    }

    [__DynamicallyInvokable]
    public static Task WhenAll(params Task[] tasks)
    {
      int length = tasks != null ? tasks.Length : throw new ArgumentNullException(nameof (tasks));
      if (length == 0)
        return Task.InternalWhenAll(tasks);
      Task[] tasks1 = new Task[length];
      for (int index = 0; index < length; ++index)
        tasks1[index] = tasks[index] ?? throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
      return Task.InternalWhenAll(tasks1);
    }

    private static Task InternalWhenAll(Task[] tasks) => tasks.Length != 0 ? (Task) new Task.WhenAllPromise(tasks) : Task.CompletedTask;

    [__DynamicallyInvokable]
    public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
    {
      switch (tasks)
      {
        case Task<TResult>[] taskArray:
          return Task.WhenAll<TResult>(taskArray);
        case ICollection<Task<TResult>> tasks2:
          int num = 0;
          Task<TResult>[] tasks1 = new Task<TResult>[tasks2.Count];
          foreach (Task<TResult> task in tasks)
            tasks1[num++] = task != null ? task : throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
          return Task.InternalWhenAll<TResult>(tasks1);
        case null:
          throw new ArgumentNullException(nameof (tasks));
        default:
          List<Task<TResult>> taskList = new List<Task<TResult>>();
          foreach (Task<TResult> task in tasks)
          {
            if (task == null)
              throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
            taskList.Add(task);
          }
          return Task.InternalWhenAll<TResult>(taskList.ToArray());
      }
    }

    [__DynamicallyInvokable]
    public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
    {
      int length = tasks != null ? tasks.Length : throw new ArgumentNullException(nameof (tasks));
      if (length == 0)
        return Task.InternalWhenAll<TResult>(tasks);
      Task<TResult>[] tasks1 = new Task<TResult>[length];
      for (int index = 0; index < length; ++index)
        tasks1[index] = tasks[index] ?? throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
      return Task.InternalWhenAll<TResult>(tasks1);
    }

    private static Task<TResult[]> InternalWhenAll<TResult>(Task<TResult>[] tasks) => tasks.Length != 0 ? (Task<TResult[]>) new Task.WhenAllPromise<TResult>(tasks) : new Task<TResult[]>(false, new TResult[0], TaskCreationOptions.None, new CancellationToken());

    [__DynamicallyInvokable]
    public static Task<Task> WhenAny(params Task[] tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      int length = tasks.Length != 0 ? tasks.Length : throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_EmptyTaskList"), nameof (tasks));
      Task[] tasks1 = new Task[length];
      for (int index = 0; index < length; ++index)
        tasks1[index] = tasks[index] ?? throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
      return TaskFactory.CommonCWAnyLogic((IList<Task>) tasks1);
    }

    [__DynamicallyInvokable]
    public static Task<Task> WhenAny(IEnumerable<Task> tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      List<Task> tasks1 = new List<Task>();
      foreach (Task task in tasks)
      {
        if (task == null)
          throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_NullTask"), nameof (tasks));
        tasks1.Add(task);
      }
      return tasks1.Count != 0 ? TaskFactory.CommonCWAnyLogic((IList<Task>) tasks1) : throw new ArgumentException(Environment.GetResourceString("Task_MultiTaskContinuation_EmptyTaskList"), nameof (tasks));
    }

    [__DynamicallyInvokable]
    public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks) => Task.WhenAny((Task[]) tasks).ContinueWith<Task<TResult>>(Task<TResult>.TaskWhenAnyCast, new CancellationToken(), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

    [__DynamicallyInvokable]
    public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks) => Task.WhenAny((IEnumerable<Task>) tasks).ContinueWith<Task<TResult>>(Task<TResult>.TaskWhenAnyCast, new CancellationToken(), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

    [FriendAccessAllowed]
    internal static Task<TResult> CreateUnwrapPromise<TResult>(Task outerTask, bool lookForOce) => (Task<TResult>) new UnwrapPromise<TResult>(outerTask, lookForOce);

    internal virtual Delegate[] GetDelegateContinuationsForDebugger() => this.m_continuationObject != this ? Task.GetDelegatesFromContinuationObject(this.m_continuationObject) : (Delegate[]) null;

    internal static Delegate[] GetDelegatesFromContinuationObject(object continuationObject)
    {
      if (continuationObject != null)
      {
        if (continuationObject is Action action)
          return new Delegate[1]
          {
            (Delegate) AsyncMethodBuilderCore.TryGetStateMachineForDebugger(action)
          };
        if (continuationObject is TaskContinuation taskContinuation)
          return taskContinuation.GetDelegateContinuationsForDebugger();
        if (continuationObject is Task task)
        {
          Delegate[] continuationsForDebugger = task.GetDelegateContinuationsForDebugger();
          if (continuationsForDebugger != null)
            return continuationsForDebugger;
        }
        if (continuationObject is ITaskCompletionAction completionAction)
          return new Delegate[1]
          {
            (Delegate) new Action<Task>(completionAction.Invoke)
          };
        if (continuationObject is List<object> objectList)
        {
          List<Delegate> delegateList = new List<Delegate>();
          foreach (object continuationObject1 in objectList)
          {
            Delegate[] continuationObject2 = Task.GetDelegatesFromContinuationObject(continuationObject1);
            if (continuationObject2 != null)
            {
              foreach (Delegate @delegate in continuationObject2)
              {
                if ((object) @delegate != null)
                  delegateList.Add(@delegate);
              }
            }
          }
          return delegateList.ToArray();
        }
      }
      return (Delegate[]) null;
    }

    private static Task GetActiveTaskFromId(int taskId)
    {
      Task activeTaskFromId = (Task) null;
      Task.s_currentActiveTasks.TryGetValue(taskId, out activeTaskFromId);
      return activeTaskFromId;
    }

    private static Task[] GetActiveTasks() => new List<Task>((IEnumerable<Task>) Task.s_currentActiveTasks.Values).ToArray();

    internal class ContingentProperties
    {
      internal ExecutionContext m_capturedContext;
      internal volatile ManualResetEventSlim m_completionEvent;
      internal volatile TaskExceptionHolder m_exceptionsHolder;
      internal CancellationToken m_cancellationToken;
      internal Shared<CancellationTokenRegistration> m_cancellationRegistration;
      internal volatile int m_internalCancellationRequested;
      internal volatile int m_completionCountdown = 1;
      internal volatile List<Task> m_exceptionalChildren;

      internal void SetCompleted() => this.m_completionEvent?.Set();

      internal void DeregisterCancellationCallback()
      {
        if (this.m_cancellationRegistration == null)
          return;
        try
        {
          this.m_cancellationRegistration.Value.Dispose();
        }
        catch (ObjectDisposedException ex)
        {
        }
        this.m_cancellationRegistration = (Shared<CancellationTokenRegistration>) null;
      }
    }

    private sealed class SetOnInvokeMres : ManualResetEventSlim, ITaskCompletionAction
    {
      internal SetOnInvokeMres()
        : base(false, 0)
      {
      }

      public void Invoke(Task completingTask) => this.Set();
    }

    private sealed class SetOnCountdownMres : ManualResetEventSlim, ITaskCompletionAction
    {
      private int _count;

      internal SetOnCountdownMres(int count) => this._count = count;

      public void Invoke(Task completingTask)
      {
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        this.Set();
      }
    }

    private sealed class DelayPromise : Task<VoidTaskResult>
    {
      internal readonly CancellationToken Token;
      internal CancellationTokenRegistration Registration;
      internal Timer Timer;

      internal DelayPromise(CancellationToken token)
      {
        this.Token = token;
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, this.Id, "Task.Delay", 0UL);
        if (!Task.s_asyncDebuggingEnabled)
          return;
        Task.AddToActiveTasks((Task) this);
      }

      internal void Complete()
      {
        bool flag;
        if (this.Token.IsCancellationRequested)
        {
          flag = this.TrySetCanceled(this.Token);
        }
        else
        {
          if (AsyncCausalityTracer.LoggingOn)
            AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks(this.Id);
          flag = this.TrySetResult(new VoidTaskResult());
        }
        if (!flag)
          return;
        if (this.Timer != null)
          this.Timer.Dispose();
        this.Registration.Dispose();
      }
    }

    private sealed class WhenAllPromise : Task<VoidTaskResult>, ITaskCompletionAction
    {
      private readonly Task[] m_tasks;
      private int m_count;

      internal WhenAllPromise(Task[] tasks)
      {
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, this.Id, "Task.WhenAll", 0UL);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        this.m_tasks = tasks;
        this.m_count = tasks.Length;
        foreach (Task task in tasks)
        {
          if (task.IsCompleted)
            this.Invoke(task);
          else
            task.AddCompletionAction((ITaskCompletionAction) this);
        }
      }

      public void Invoke(Task completedTask)
      {
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationRelation(CausalityTraceLevel.Important, this.Id, CausalityRelation.Join);
        if (Interlocked.Decrement(ref this.m_count) != 0)
          return;
        List<ExceptionDispatchInfo> exceptionObject = (List<ExceptionDispatchInfo>) null;
        Task task1 = (Task) null;
        for (int index = 0; index < this.m_tasks.Length; ++index)
        {
          Task task2 = this.m_tasks[index];
          if (task2.IsFaulted)
          {
            if (exceptionObject == null)
              exceptionObject = new List<ExceptionDispatchInfo>();
            exceptionObject.AddRange((IEnumerable<ExceptionDispatchInfo>) task2.GetExceptionDispatchInfos());
          }
          else if (task2.IsCanceled && task1 == null)
            task1 = task2;
          if (task2.IsWaitNotificationEnabled)
            this.SetNotificationForWaitCompletion(true);
          else
            this.m_tasks[index] = (Task) null;
        }
        if (exceptionObject != null)
          this.TrySetException((object) exceptionObject);
        else if (task1 != null)
        {
          this.TrySetCanceled(task1.CancellationToken, (object) task1.GetCancellationExceptionDispatchInfo());
        }
        else
        {
          if (AsyncCausalityTracer.LoggingOn)
            AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks(this.Id);
          this.TrySetResult(new VoidTaskResult());
        }
      }

      internal override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion(this.m_tasks);
    }

    private sealed class WhenAllPromise<T> : Task<T[]>, ITaskCompletionAction
    {
      private readonly Task<T>[] m_tasks;
      private int m_count;

      internal WhenAllPromise(Task<T>[] tasks)
      {
        this.m_tasks = tasks;
        this.m_count = tasks.Length;
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, this.Id, "Task.WhenAll", 0UL);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        foreach (Task<T> task in tasks)
        {
          if (task.IsCompleted)
            this.Invoke((Task) task);
          else
            task.AddCompletionAction((ITaskCompletionAction) this);
        }
      }

      public void Invoke(Task ignored)
      {
        if (AsyncCausalityTracer.LoggingOn)
          AsyncCausalityTracer.TraceOperationRelation(CausalityTraceLevel.Important, this.Id, CausalityRelation.Join);
        if (Interlocked.Decrement(ref this.m_count) != 0)
          return;
        T[] result = new T[this.m_tasks.Length];
        List<ExceptionDispatchInfo> exceptionObject = (List<ExceptionDispatchInfo>) null;
        Task task1 = (Task) null;
        for (int index = 0; index < this.m_tasks.Length; ++index)
        {
          Task<T> task2 = this.m_tasks[index];
          if (task2.IsFaulted)
          {
            if (exceptionObject == null)
              exceptionObject = new List<ExceptionDispatchInfo>();
            exceptionObject.AddRange((IEnumerable<ExceptionDispatchInfo>) task2.GetExceptionDispatchInfos());
          }
          else if (task2.IsCanceled)
          {
            if (task1 == null)
              task1 = (Task) task2;
          }
          else
            result[index] = task2.GetResultCore(false);
          if (task2.IsWaitNotificationEnabled)
            this.SetNotificationForWaitCompletion(true);
          else
            this.m_tasks[index] = (Task<T>) null;
        }
        if (exceptionObject != null)
          this.TrySetException((object) exceptionObject);
        else if (task1 != null)
        {
          this.TrySetCanceled(task1.CancellationToken, (object) task1.GetCancellationExceptionDispatchInfo());
        }
        else
        {
          if (AsyncCausalityTracer.LoggingOn)
            AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, this.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks(this.Id);
          this.TrySetResult(result);
        }
      }

      internal override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion((Task[]) this.m_tasks);
    }
  }
}
