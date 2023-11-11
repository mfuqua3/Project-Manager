import React, {ReactNode} from "react";
import {Box, Button, Divider, List, ListItem, ListItemButton, ListItemIcon, ListItemText} from "@mui/material";
import HomeIcon from '@mui/icons-material/Home';
import {useNavigate} from "react-router-dom";
import {useDrawer} from "../../utils/drawer";
import {ProjectManagerRoutes} from "../../App.Routes";
import LockPersonIcon from '@mui/icons-material/LockPerson';

interface DrawerItem {
    icon: ReactNode;
    title: string;
    navigate: string;
}

function NavigationDrawer() {
    const navigate = useNavigate();
    const {close} = useDrawer();
    const items: DrawerItem[] = [
        {icon: <HomeIcon/>, title: "Home", navigate: ProjectManagerRoutes.home}
    ]

    function handleClick(item: DrawerItem) {
        navigate(item.navigate);
        close();
    }

    return (
        <Box display={"flex"} flexDirection={"column"} justifyContent={"space-between"} height={"100%"}>
            <List disablePadding>
                <ListItem sx={(theme)=>({
                    backgroundColor: theme.palette.secondary.main,
                    marginBottom: '2px',
                    boxShadow: 4
                })}>
                    <Button>
                        <Box component={"img"} src={"/drawer_banner.png"} width={"100%"}
                             onClick={() => navigate(ProjectManagerRoutes.home)}/>
                    </Button>
                </ListItem>
                <Divider/>
                {items.map((item) =>
                    <ListItem key={item.title} disablePadding>
                        <ListItemButton onClick={() => handleClick(item)}>
                            <ListItemIcon>
                                {item.icon}
                            </ListItemIcon>
                            <ListItemText primary={item.title}/>
                        </ListItemButton>
                    </ListItem>)}
            </List>
            <List>
                <Divider/>
                <ListItem>
                    <ListItemButton onClick={() => navigate(ProjectManagerRoutes.login)}>
                        <ListItemIcon>
                            <LockPersonIcon/>
                        </ListItemIcon>
                        <ListItemText primary={"Login"}/>
                    </ListItemButton>
                </ListItem>
            </List>
        </Box>
    )
}


export default React.memo(NavigationDrawer);