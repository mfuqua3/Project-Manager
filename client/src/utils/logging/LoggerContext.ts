// src/LoggerContext.tsx
import {createContext} from "react";
import {LoggingAdapter} from "./LoggingAdapter";

export const LoggerContext = createContext<LoggingAdapter[]>([]);
