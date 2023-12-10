import React, {createContext, ReactNode, useContext, useState} from "react";

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

    const toggleOpen = () => {
        setIsOpen(!isOpen);
    };

    return (
        <SideMenuContext.Provider value={{isOpen, toggleOpen}}>
            {children}
        </SideMenuContext.Provider>
    );
};