import { defineConfig } from "vite";

export default defineConfig({
    build: {
        lib: {
            entry: "src/entry.ts",
            formats: ["es"],
            name : "TheDashboard.Umbraco"
        },
        outDir: "../wwwroot/App_Plugins/Our.Umbraco.TheDashboard/dist",
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/]
        },
    }
});
