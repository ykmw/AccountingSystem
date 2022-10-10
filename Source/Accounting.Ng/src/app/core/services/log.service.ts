import { Injectable } from "@angular/core";

export enum LogLevel {
  Trace,
  Debug,
  Information,
  Warning,
  Error,
}

@Injectable()
export class LogService {
  log(context: string, level: LogLevel, msg: any) {
    switch (level) {
      case LogLevel.Trace:
        console.log(
          "Level:",
          LogLevel[level],
          " Context: ",
          context,
          "\n Error: ",
          JSON.stringify(msg)
        );
        break;
      case LogLevel.Debug:
        console.debug(
          "Level:",
          LogLevel[level],
          " Context: ",
          context,
          "\n Error: ",
          JSON.stringify(msg)
        );
        break;
      case LogLevel.Information:
        console.info(
          "Level:",
          LogLevel[level],
          " Context: ",
          context,
          "\n Error: ",
          JSON.stringify(msg)
        );
        break;
      case LogLevel.Warning:
        console.warn(
          "Level:",
          LogLevel[level],
          " Context: ",
          context,
          "\n Error: ",
          JSON.stringify(msg)
        );
        break;
      case LogLevel.Error:
        console.error(
          "Level:",
          LogLevel[level],
          " Context: ",
          context,
          "\n Error: ",
          JSON.stringify(msg)
        );
        break;
      default:
        console.log("Context: ", context, "\n Error: ", JSON.stringify(msg));
        break;
    }
  }
}
