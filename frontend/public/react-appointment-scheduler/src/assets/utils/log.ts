export type LogPriority = `v` | `d` | `i` | `w` | `e`;

type LogMethod = {
    (...data: any[]): void;
    (message?: any, ...optionalParams: any[]): void;
}

const LogMethods: { [priority in LogPriority]: LogMethod } = {
    v: console.log, d: console.debug,
    i: console.info, w: console.warn, e: console.error
};

function _log(method: LogMethod, tag: string, ...data: any[]): void {
    method(`[${tag}]#${arguments.callee.caller.caller.name
        }${data.length === 0 ? `` : `: ${data[0]}`}`, ...data.slice(1));
}

type BindedLog = (...args: any[]) => void;

type BindedTagLog = {
    (priority: LogPriority, ...data: any[]): void;
    bindPriority(priority: LogPriority): BindedLog;
};

type BindedPriorityLog = {
    (tag: string, ...data: any[]): void;
    bindTag(tag: string): BindedLog;
};

type LogPriorityConstants = {
    VERBOSE: `v`;
    DEBUG: `d`;
    INFO: `i`;
    WARN: `w`;
    ERROR: `e`;
}

type Log = {
    (priority: LogPriority, tag: string, ...data: any[]): void;
    bindTag(tag: string): BindedTagLog;
    bindPriority(priority: LogPriority): BindedPriorityLog;
} & LogPriorityConstants

const log: Log = function (priority: LogPriority, tag: string, ...data: any[]): void {
    _log.call(LogMethods[priority], tag, data);
}

log.bindTag = function (tag: string): BindedTagLog {
    const log: BindedTagLog = function (priority: LogPriority, ...data: any[]): void {
        _log.call(LogMethods[priority], tag, data);
    };
    log.bindPriority = function (priority: LogPriority): BindedLog {
        return log.bind(this, priority);
    }
    return log;
};

log.bindPriority = function (priority: LogPriority): BindedPriorityLog {
    const log: BindedPriorityLog = function (tag: string, ...data: any[]): void {
        _log.call(LogMethods[priority], tag, data);
    }
    log.bindTag = function (tag: string): BindedLog {
        return log.bind(this, tag);
    }
    return log;
};

log.VERBOSE = `v`;
log.DEBUG = `d`;
log.INFO = `i`;
log.WARN = `w`;
log.ERROR = `e`;

export default log;