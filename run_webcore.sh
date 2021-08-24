#!/usr/bin/env bash

# This is a temporary script to run WebCORE, since no command line option is available for now

# Change this as needed
cd /media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchcore-web/webcore-server

# Sadly, this needs to be on one long line. Get this command from Eclipse run configuration
/usr/local/jdk-15.0.2/bin/java -Dfile.encoding=UTF-8 -classpath /media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchcore-web/webcore-server/target/classes:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.classdiagram/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.ecore_2.23.0.v20200630-0516.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.common_2.22.0.v20210114-1734.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.ecore.xmi_2.16.0.v20190528-0725.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.runtime_3.20.100.v20210111-0815.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.osgi_3.16.200.v20210122-1907.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.fx.osgi_3.7.0.202010120720.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.osgi.compatibility.state_1.2.200.v20200915-2015.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.equinox.common_3.14.0.v20201102-2053.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.jobs_3.10.1100.v20210111-0815.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.equinox.registry_3.10.0.v20201107-1818.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.equinox.preferences_3.8.100.v20201102-2042.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.contenttype_3.7.900.v20210111-0918.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.equinox.app_1.5.0.v20200717-0620.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.commons/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.uml2.uml.resources_5.5.0.v20210228-1829.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.uml2.uml_5.5.0.v20210228-1829.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.uml2.common_2.5.0.v20210228-1829.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.uml2.types_2.5.0.v20210228-1829.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mapping.ecore2xml_2.11.0.v20180706-1146.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.uml2.uml.profile.standard_1.5.0.v20210228-1829.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.ecore_3.16.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl_3.16.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/lpg.runtime.java_2.0.17.v201004271640.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.common_1.8.600.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.xtext.oclinecore_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.xtext.essentialocl_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.xtext.base_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext_2.25.0.v20210301-0843.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.antlr.runtime_3.2.0.v201101311130.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/com.google.inject_3.0.0.v201605172100.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/com.google.inject.multibindings_3.0.0.v201605172100.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe.core_1.6.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.apache.commons.cli_1.4.0.v20200417-1444.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe2.runtime_2.12.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe.utils_1.6.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.util_2.25.0.v20210301-0843.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/com.google.guava_30.1.0.v20210127-2300.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/javax.inject_1.0.0.v20091030.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.pivot_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.jdt.annotation_2.2.600.v20200408-1511.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.edit_2.16.0.v20190920-0401.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.ecore.change_2.14.0.v20190528-0725.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.examples.xtext.serializer_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.xtext.oclstdlib_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ocl.xtext.completeocl_1.14.0.v20210310-0557.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.apache.log4j_1.2.15.v201012070815.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.apache.commons.io_2.6.0.v20190123-2029.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.controller/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.classdiagram.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.classdiagram.controller/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.weaver/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.evaluator/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.evaluator/lib/familiar-bridge-0.0.1.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.junit_4.13.0.v20200204-1500.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.hamcrest.core_1.3.0.v20180420-1519.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf_2.8.0.v20180706-1146.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.core.language/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/core/ca.mcgill.sel.perspective/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.classloader/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.classloader/lib/asm-5.0.3.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.controller/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.controller/tests/lib/assertj-core-3.5.1.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.expressions/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.xbase_2.25.0.v20210301-0909.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.common.types_2.25.0.v20210301-0909.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtend.lib_2.25.0.v20210301-0821.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.xbase.lib_2.25.0.v20210301-0821.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtend.lib.macro_2.25.0.v20210301-0821.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.junit4_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.xtext.generator_2.25.0.v20210301-0843.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.codegen.ecore_2.25.0.v20201231-0738.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.codegen_2.21.0.v20200708-0547.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe2.launch_2.12.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe2.language_2.12.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.mwe2.lib_2.12.1.v20210218-2134.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.objectweb.asm_9.1.0.v20210209-1849.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.apache.commons.logging_1.2.0.v20180409-1502.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/com.ibm.icu_67.1.0.v20200706-1749.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.generator_2.25.0.v20210301-0909.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtend_2.2.0.v201605260315.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xpand_2.2.0.v201605260315.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtend.typesystem.emf_2.2.0.v201605260315.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.expressions.ide/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ide_2.25.0.v20210301-0843.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.xbase.ide_2.25.0.v20210301-0909.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.expressions.tests/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.antlr.runtime_4.7.2.v20200218-0804.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.testing_2.25.0.v20210301-0843.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.expressions.ui/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ui_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.resources_3.13.900.v20201105-1507.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui_3.119.0.v20210111-1350.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.swt_3.116.0.v20210129-1709.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.swt.browser.chromium.gtk.linux.x86_64_3.116.0.v20210129-1709.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.swt.gtk.linux.x86_64_3.116.0.v20210129-1709.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.jface_3.22.100.v20210126-0831.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.commands_3.9.800.v20201021-1339.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui.workbench_3.122.100.v20210128-2029.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.e4.ui.workbench3_0.15.500.v20201021-1339.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui.editors_3.14.0.v20210121-1258.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.core.filebuffers_3.6.1100.v20201029-1159.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.jface.text_3.17.0.v20210125-1136.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.text_3.11.0.v20210123-0709.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.edit.ui_2.19.0.v20200917-1406.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui.views_3.11.0.v20210111-1351.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.common.ui_2.18.0.v20190507-0402.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ltk.core.refactoring_3.11.300.v20210112-0706.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ui.shared_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ui.codetemplates.ui_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ui.codetemplates_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.ui.codetemplates.ide_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui.ide_3.18.100.v20210122-1536.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.e4.ui.ide_3.15.200.v20210108-1832.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.compare_3.7.1300.v20210114-0707.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.compare.core_3.6.1000.v20201020-1107.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.xtext.builder_2.25.0.v20210301-0928.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.debug.ui_3.14.800.v20210121-1016.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.debug.core_3.17.100.v20210120-0743.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.ui.workbench.texteditor_3.16.0.v20210120-0733.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.generator/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.acceleo.engine_3.7.11.202102190929.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.acceleo.common_3.7.11.202102190929.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.acceleo.model_3.7.11.202102190929.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.acceleo.profiler_3.7.11.202102190929.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.usecases/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.validator/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.weaver/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.usecases.language/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.usecases.edit/bin:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03/plugins/org.eclipse.emf.ecore.edit_2.13.0.v20190822-1451.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.restif/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.restif.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.restif.controller/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.environmentmodel/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.environmentmodel.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.environmentmodel.language/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.operationmodel/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.operationmodel.edit/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.operationmodel.language/bin:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/jgraphx.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/junit-4.13.2.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/mt4j.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/org.abego.treelayout.core.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/jogl/gluegen-rt.jar:/media/Storage/McGill/MastersThesis/TouchCORE-2021-05/touchram/ca.mcgill.sel.ram.gui/lib/jogl/jogl-all.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/emfjson/emfjson-jackson/1.2.0/emfjson-jackson-1.2.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/core/jackson-databind/2.9.6/jackson-databind-2.9.6.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/core/jackson-annotations/2.9.0/jackson-annotations-2.9.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/json/json/20180813/json-20180813.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter-websocket/2.0.5.RELEASE/spring-boot-starter-websocket-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter/2.0.5.RELEASE/spring-boot-starter-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot/2.0.5.RELEASE/spring-boot-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-autoconfigure/2.0.5.RELEASE/spring-boot-autoconfigure-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter-logging/2.0.5.RELEASE/spring-boot-starter-logging-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/ch/qos/logback/logback-classic/1.2.3/logback-classic-1.2.3.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/ch/qos/logback/logback-core/1.2.3/logback-core-1.2.3.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/logging/log4j/log4j-to-slf4j/2.10.0/log4j-to-slf4j-2.10.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/logging/log4j/log4j-api/2.10.0/log4j-api-2.10.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/slf4j/jul-to-slf4j/1.7.25/jul-to-slf4j-1.7.25.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/javax/annotation/javax.annotation-api/1.3.2/javax.annotation-api-1.3.2.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/yaml/snakeyaml/1.19/snakeyaml-1.19.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter-web/2.0.5.RELEASE/spring-boot-starter-web-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter-json/2.0.5.RELEASE/spring-boot-starter-json-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/datatype/jackson-datatype-jdk8/2.9.6/jackson-datatype-jdk8-2.9.6.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/datatype/jackson-datatype-jsr310/2.9.6/jackson-datatype-jsr310-2.9.6.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/module/jackson-module-parameter-names/2.9.6/jackson-module-parameter-names-2.9.6.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/boot/spring-boot-starter-tomcat/2.0.5.RELEASE/spring-boot-starter-tomcat-2.0.5.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/tomcat/embed/tomcat-embed-core/8.5.34/tomcat-embed-core-8.5.34.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/tomcat/embed/tomcat-embed-el/8.5.34/tomcat-embed-el-8.5.34.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/tomcat/embed/tomcat-embed-websocket/8.5.34/tomcat-embed-websocket-8.5.34.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/hibernate/validator/hibernate-validator/6.0.12.Final/hibernate-validator-6.0.12.Final.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/javax/validation/validation-api/2.0.1.Final/validation-api-2.0.1.Final.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/jboss/logging/jboss-logging/3.3.2.Final/jboss-logging-3.3.2.Final.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/classmate/1.3.4/classmate-1.3.4.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-web/5.0.9.RELEASE/spring-web-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-webmvc/5.0.9.RELEASE/spring-webmvc-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-aop/5.0.9.RELEASE/spring-aop-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-expression/5.0.9.RELEASE/spring-expression-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-messaging/5.0.9.RELEASE/spring-messaging-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-beans/5.0.9.RELEASE/spring-beans-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-websocket/5.0.9.RELEASE/spring-websocket-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-context/5.0.9.RELEASE/spring-context-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/webjars/webjars-locator-core/0.35/webjars-locator-core-0.35.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/slf4j/slf4j-api/1.7.25/slf4j-api-1.7.25.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/commons/commons-lang3/3.7/commons-lang3-3.7.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/apache/commons/commons-compress/1.9/commons-compress-1.9.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/fasterxml/jackson/core/jackson-core/2.9.6/jackson-core-2.9.6.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/webjars/sockjs-client/1.0.2/sockjs-client-1.0.2.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/webjars/stomp-websocket/2.3.3/stomp-websocket-2.3.3.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/webjars/bootstrap/3.3.7/bootstrap-3.3.7.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/webjars/jquery/3.1.0/jquery-3.1.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/guava/guava/27.1-jre/guava-27.1-jre.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/guava/failureaccess/1.0.1/failureaccess-1.0.1.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/guava/listenablefuture/9999.0-empty-to-avoid-conflict-with-guava/listenablefuture-9999.0-empty-to-avoid-conflict-with-guava.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/code/findbugs/jsr305/3.0.2/jsr305-3.0.2.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/checkerframework/checker-qual/2.5.2/checker-qual-2.5.2.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/errorprone/error_prone_annotations/2.2.0/error_prone_annotations-2.2.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/j2objc/j2objc-annotations/1.1/j2objc-annotations-1.1.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/codehaus/mojo/animal-sniffer-annotations/1.17/animal-sniffer-annotations-1.17.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/com/google/code/gson/gson/2.8.5/gson-2.8.5.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/hamcrest/hamcrest-core/1.3/hamcrest-core-1.3.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-core/5.0.9.RELEASE/spring-core-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/springframework/spring-jcl/5.0.9.RELEASE/spring-jcl-5.0.9.RELEASE.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/eclipse/emf/org.eclipse.emf.edit/2.15.0/org.eclipse.emf.edit-2.15.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/eclipse/emf/org.eclipse.emf.common/2.22.0/org.eclipse.emf.common-2.22.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/org/eclipse/emf/org.eclipse.emf.ecore/2.24.0/org.eclipse.emf.ecore-2.24.0.jar:/media/Storage/McGill/MastersThesis/eclipse-committers-2021-03-home/.m2/repository/junit/junit/4.12/junit-4.12.jar -XX:+ShowCodeDetailsInExceptionMessages ca.mcgill.sel.webcore.WebCore
