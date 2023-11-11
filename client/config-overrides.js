/* eslint-disable @typescript-eslint/no-var-requires */
const NodePolyfillPlugin = require("node-polyfill-webpack-plugin");
const ModuleScopePlugin = require('react-dev-utils/ModuleScopePlugin');

module.exports = function override(config, env){
    config.resolve.plugins = config.resolve.plugins.filter(plugin => !(plugin instanceof ModuleScopePlugin));

    config.plugins.push(new NodePolyfillPlugin());
    return config;
}