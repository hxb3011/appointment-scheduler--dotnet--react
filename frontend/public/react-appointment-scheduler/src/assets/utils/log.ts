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
    const error = new Error();
    let stack = error.stack;
    let methodName: string | undefined = undefined;
    if (stack) {
        if (!methodName) {
            let count = 3;
            for (const match of stack.matchAll(/^\s*at\s+(.*)\s*\(/gm)) {
                if (--count === 0) {
                    methodName = match[1];
                    break;
                }
            }
        }
        if (!methodName) {
            let count = 3;
            for (const match of stack.matchAll(/^\s*(.*)\s*@/gm)) {
                if (--count === 0) {
                    methodName = match[1];
                    break;
                }
            }
        }
    }
    method(`[${tag}]${methodName ? `#${methodName}` : ``}${data.length ? `: ${data[0]}` : ``}`, ...data.slice(1));
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
    (this: any, priority: LogPriority, tag: string, ...data: any[]): void;
    bindTag(this: any, tag: string): BindedTagLog;
    bindPriority(this: any, priority: LogPriority): BindedPriorityLog;
} & LogPriorityConstants

const log: Log = function (this: any, priority: LogPriority, tag: string, ...data: any[]): void {
    _log.call(this, LogMethods[priority], tag, data);
}

log.bindTag = function (this: any, tag: string): BindedTagLog {
    const log: BindedTagLog = function (this: any, priority: LogPriority, ...data: any[]): void {
        _log.call(this, LogMethods[priority], tag, data);
    };
    log.bindPriority = function (this: any, priority: LogPriority): BindedLog {
        return log.bind(this, priority);
    }
    return log;
};

log.bindPriority = function (this: any, priority: LogPriority): BindedPriorityLog {
    const log: BindedPriorityLog = function (this: any, tag: string, ...data: any[]): void {
        _log.call(this, LogMethods[priority], tag, data);
    }
    log.bindTag = function (this: any, tag: string): BindedLog {
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