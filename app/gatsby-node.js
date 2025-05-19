const fs = require('fs-extra');
const path = require('path');

exports.onPreBootstrap = ({ reporter }) => {
    const imageDirs = ['bot', 'landing', 'plugin'];

    imageDirs.forEach((dir) => {
        const srcDir = path.join(process.cwd(), 'src', 'images', dir);
        const publicDir = path.join(process.cwd(), 'public', 'images', dir);

        fs.ensureDirSync(srcDir);
        fs.ensureDirSync(publicDir);

        try {
            if (fs.existsSync(srcDir)) {
                fs.copySync(srcDir, publicDir, {
                    overwrite: true,
                    errorOnExist: false,
                });
                reporter.success(
                    `Successfully copied files from ${dir} directory`
                );
            }
        } catch (error) {
            reporter.error(`Error copying files from ${dir} directory:`, error);
        }
    });
};
