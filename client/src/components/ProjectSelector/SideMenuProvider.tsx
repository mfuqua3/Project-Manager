import React, {createContext, ReactNode, useContext, useState} from "react";
import {useAuth} from "../../utils/auth";

interface SideMenuContextType {
    isOpen: boolean;
    toggleOpen: () => void;
}

const SideMenuContext = createContext<SideMenuContextType | undefined>(undefined);

export const useSideMenu = () => {
    const context = useContext(SideMenuContext);
    if (!context) {
        throw new Error("useSideMenu must be used within SideMenuProvider");
    }
    return context;
};
export const SideMenuProvider = ({children}: { children: ReactNode }) => {
    const [isOpen, setIsOpen] = useState(false);
    const {isAuthenticated} = useAuth();

    const toggleOpen = () => {
        setIsOpen(!isOpen && isAuthenticated);
    };

    return (
        <SideMenuContext.Provider value={{isOpen: isOpen && isAuthenticated, toggleOpen}}>
            {children}
        </SideMenuContext.Provider>
    );
};