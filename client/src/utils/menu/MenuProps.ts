import { PopoverVirtualElement } from "@mui/material";

export interface MenuProps {
    anchorEl: any;
    isOpen: boolean;

    open(anchorEl: Element): void;

    close(): void;
}
